"use strict";

var gulp = require("gulp");

var paths = {
    webRoot: "./wwwroot/",
    configRoot: "./_config/",
    apiRoot: "./Api/",
    businessRoot: "./Business/",
    dataAccessRoot: "./DataAccess/",
    entitiesRoot: "./Entitities/",
    serviceAgentsRoot: "./ServiceAgents/",
    sharedRoot: "./Shared/",
    startupRoot: "./Startup/"
};

gulp.task("prd", function (cb) {
    // ToDo : enter task logic here
    //        this task is run by the CI system
});
