/// <binding AfterBuild='default' Clean='clean' />

var gulp = require("gulp");
var del = require("del");

var paths = {
    scripts: ["src/scripts/**/*.ts"]
};

gulp.task("clean", function () {
    return del(["dist/scripts/**/*"]);
});

gulp.task("default", function () {
    gulp.src(paths.scripts).pipe(gulp.dest("dist/scripts"));
});