// Entity Framework
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

// System Usings
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Linq.Expressions;
global using System.Reflection;

// Project Usings - Models
global using Servus.Models.Enums;
global using Servus.Models.Entities;
global using Servus.Models.DTOs;
global using Servus.Models.Interfaces;

// Project Usings - Data
global using Servus.Data.Contexts;
global using Servus.ViewModels;
global using Servus.Services;

// Project Usings - Extensions
global using Servus.Extensions;

// MAUI Usings
global using Microsoft.Maui.Controls;
global using Microsoft.Maui.Storage;

// .NET Extensions
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Caching.Memory;