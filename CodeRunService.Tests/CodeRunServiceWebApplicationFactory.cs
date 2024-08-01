using CodeRunService.Tests;
using Common;
using CodeRunService.Infrastructure;
using DomainEntities;
using Generics.Enums;
using MongoDB.Bson;

namespace CodeRunService.Tests;

public class CodeRunServiceWebApplicationFactory<TStartup>()
    : CustomWebApplicationFactory<TStartup, CodeRunDbContext>()
    where TStartup : class;