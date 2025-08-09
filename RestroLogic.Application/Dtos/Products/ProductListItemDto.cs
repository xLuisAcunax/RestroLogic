﻿namespace RestroLogic.Application.Dtos.Products
{
    public class ProductListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
