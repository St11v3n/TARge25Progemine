using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.Dto.CocktailDtos;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface ICocktailServices
    {
        Task<CocktailRootDto> GetCocktails(CocktailResultDto dto);
    }
}