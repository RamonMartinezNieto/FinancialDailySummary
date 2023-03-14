//System
global using System.Threading.Tasks;
global using System.ComponentModel;
global using System.Net.Http.Json;
global using System.Threading;
global using System.Net.Http;
global using System.Linq;
global using System.Text;
global using System.IO;
global using System;

//Microsoft 
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

//Internal
global using FinancialDailySummary.ConfigurationModels;
global using FinancialDailySummary.ExtensionMethods;
global using FinancialDailySummary.Models.Yahoo;
global using FinancialDailySummary.Attributes;
global using FinancialDailySummary.Services;
global using FinancialDailySummary.Clients;
global using FinancialDailySummary.Enums;
