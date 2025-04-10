﻿using AutoMapper;
using Microservices.Services.ShoppingCartAPI.Models;
using Microservices.Services.ShoppingCartAPI.Models.Dto;

namespace Microservices.Services.ShopingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
