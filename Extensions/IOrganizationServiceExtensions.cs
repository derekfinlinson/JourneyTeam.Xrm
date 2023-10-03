using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public static partial class IOrganizationServiceExtensions
    {
        /// <summary>
        /// Creates a list of records.
        /// </summary>
        /// <param name="entities">A list of entity instances that contains the properties to set in the newly created records.</param>
        public static IEnumerable<Entity> Create(this IOrganizationService service, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Id = service.Create(entity);
            }

            return entities;
        }

        /// <summary>
        /// Updates a list of existing records.
        /// </summary>
        /// <param name="entities">A list of entity instances that have one or more properties set to be updated in the records.</param>
        public static void Update(this IOrganizationService service, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                service.Update(entity);
            }
        }

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="reference">Entity record to delete.</param>
        public static void Delete(this IOrganizationService service, Entity entity)
        {
            service.Delete(entity.LogicalName, entity.Id);
        }

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="reference">EntityReference record to delete.</param>
        public static void Delete(this IOrganizationService service, EntityReference reference)
        {
            service.Delete(reference.LogicalName, reference.Id);
        }

        /// <summary>
        /// Deletes a list of records.
        /// </summary>
        /// <param name="entities">A list of entity reference records to delete.</param>
        public static void Delete(this IOrganizationService service, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                service.Delete(entity);
            }
        }

        /// <summary>
        /// Deletes a list of records.
        /// </summary>
        /// <param name="entities">A list of entity reference records to delete.</param>
        public static void Delete(this IOrganizationService service, IEnumerable<EntityReference> references)
        {
            foreach (var reference in references)
            {
                service.Delete(reference);
            }
        }

        /// <summary>
        /// Retrieve entity from entity reference
        /// </summary>
        /// <param name="reference">Entity reference</param>
        /// <param name="columnSet">Columns to retrieve</param>
        /// <returns></returns>
        public static Entity Retrieve(this IOrganizationService service, EntityReference reference, ColumnSet columnSet)
        {
            return service.Retrieve(reference.LogicalName, reference.Id, columnSet);
        }

        /// <summary>
        /// Retrieve multiple based on string FetchXML
        /// </summary>
        /// <param name="fetch"></param>
        /// <returns></returns>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, string fetch)
        {
            return service.RetrieveMultiple(new FetchExpression(fetch));
        }

        /// <summary>
        /// Creates a link between records.
        /// </summary>
        /// <param name="reference">EntityReference to disassociate.</param>
        /// <param name="relationship">The name of the relationship to be used to create the link.</param>
        /// <param name="relatedEntities">A collection of entity references (references to records) to be associated.</param>
        public static void Associate(this IOrganizationService service, EntityReference reference, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            service.Associate(reference.LogicalName, reference.Id, relationship, relatedEntities);
        }

        /// <summary>
        /// Deletes a link between records.
        /// </summary>
        /// <param name="reference">EntityReference to disassociate.</param>
        /// <param name="relationship">The name of the relationship to be used to remove the link.</param>
        /// <param name="relatedEntities">A collection of entity references (references to records) to be disassociated.</param>
        public static void Disassociate(this IOrganizationService service, EntityReference reference, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            service.Disassociate(reference.LogicalName, reference.Id, relationship, relatedEntities);
        }

        /// <summary>
        /// Upload a file to a file type column
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="fileAttributeName">File column name</param>
        /// <param name="fileName">Filename</param>
        /// <param name="fileMimeType">File type</param>
        /// <param name="file">File as Base64 string</param>
        public static void UploadFile(this IOrganizationService service, Entity entity, string fileAttributeName, string fileName, string fileMimeType, string file)
        {
            // Initialize the upload
            var uploadRequest = new InitializeFileBlocksUploadRequest
            {
                Target = entity.ToEntityReference(),
                FileAttributeName = fileAttributeName,
                FileName = fileName
            };

            var uploadResponse = (InitializeFileBlocksUploadResponse)service.Execute(uploadRequest);

            string fileContinuationToken = uploadResponse.FileContinuationToken;

            // Capture blockids while uploading
            var blockIds = new List<string>();

            using (var uploadFileStream = new MemoryStream(Convert.FromBase64String(file)))
            {
                int blockSize = 4 * 1024 * 1024; // 4 MB

                byte[] buffer = new byte[blockSize];
                int bytesRead = 0;

                // The number of iterations that will be required:
                // int blocksCount = (int)Math.Ceiling(fileSize / (float)blockSize);
                int blockNumber = 0;

                // While there is unread data from the file
                while ((bytesRead = uploadFileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // The file or final block may be smaller than 4MB
                    if (bytesRead < buffer.Length)
                    {
                        Array.Resize(ref buffer, bytesRead);
                    }

                    blockNumber++;

                    string blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

                    blockIds.Add(blockId);

                    // Prepare the request
                    var uploadBlockRequest = new UploadBlockRequest()
                    {
                        BlockData = buffer,
                        BlockId = blockId,
                        FileContinuationToken = fileContinuationToken,
                    };

                    // Send the request
                    service.Execute(uploadBlockRequest);
                }
            }

            // Commit the upload
            var commitFileBlocksUploadRequest = new CommitFileBlocksUploadRequest()
            {
                BlockList = blockIds.ToArray(),
                FileContinuationToken = fileContinuationToken,
                FileName = fileName,
                MimeType = fileMimeType
            };

            service.Execute(commitFileBlocksUploadRequest);
        }
    }
}