﻿using AutoMapper;
using Newtonsoft.Json.Serialization;
using Salon.API.DTO;
using Salon.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Salon.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            CreateMaps();
        }
        public static void CreateMaps()
        {
            Mapper.CreateMap<Appointment, AppointmentDTO>();
            Mapper.CreateMap<Appointment, AppointmentDTO>();
            Mapper.CreateMap<Customer, CustomerDTO>();
            Mapper.CreateMap<Product, ProductDTO>();
            Mapper.CreateMap<Service, ServiceDTO>();
            Mapper.CreateMap<Stylist, StylistDTO>();
            Mapper.CreateMap<Transaction, TransactionDTO>();
        }
    }
}
