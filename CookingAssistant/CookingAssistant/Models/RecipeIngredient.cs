﻿namespace CookingAssistant.Models
{
    public class RecipeIngredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public int Ammount { get; set; }
        public Units Unit { get; set; }
        
        public RecipeIngredient(int ingredientId, string name, int amount, Units unit)
        {
            IngredientId = ingredientId;
            Name = name;
            Ammount = amount;
            Unit = unit;
        }
    }
}