using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfRecipeApp
{
    public partial class MainWindow : Window
    {
        // Define the Ingredient class to represent ingredients of a recipe
        public class Ingredient
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
            public int Calories { get; set; }
            public string FoodGroup { get; set; }

            // Constructor to initialize an Ingredient with specified properties
            public Ingredient(string name, int quantity, int calories, string foodGroup)
            {
                Name = name;
                Quantity = quantity;
                Calories = calories;
                FoodGroup = foodGroup;
            }
        }

        // Define the Recipe class to represent a recipe with its name and ingredients
        public class Recipe
        {
            public string Name { get; set; }
            public List<Ingredient> Ingredients { get; set; }

            // Constructor to initialize a Recipe with a name and an empty list of ingredients
            public Recipe(string name)
            {
                Name = name;
                Ingredients = new List<Ingredient>();
            }
        }

        // Manage a collection of recipes and provide methods to add and filter recipes
        public class RecipeManager
        {
            public List<Recipe> Recipes { get; private set; }

            // Constructor to initialize the RecipeManager with an empty list of recipes
            public RecipeManager()
            {
                Recipes = new List<Recipe>();
            }

            // Method to add a new recipe to the Recipes list
            public void AddRecipe(Recipe recipe)
            {
                Recipes.Add(recipe);
            }

            // Method to filter recipes based on ingredient name containing filter text
            public List<Recipe> FilterRecipes(string ingredientName)
            {
                // Use LINQ to filter recipes based on ingredient name
                return Recipes.Where(r => r.Ingredients.Any(i => i.Name.Contains(ingredientName, StringComparison.OrdinalIgnoreCase))).ToList();
            }
        }

        private RecipeManager recipeManager; // Instance of RecipeManager to manage recipes

        // Constructor for MainWindow
        public MainWindow()
        {
            InitializeComponent();

            // Initialize the RecipeManager instance
            recipeManager = new RecipeManager();

            // Set the ItemsSource of RecipesListBox to the Recipes list in RecipeManager
            RecipesListBox.ItemsSource = recipeManager.Recipes;
        }

        // Event handler for Add Recipe button click
        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            Recipe newRecipe = new Recipe("New Recipe"); // Create a new Recipe object

            // Prompt user to enter recipe name
            string recipeName = Microsoft.VisualBasic.Interaction.InputBox("Enter Recipe Name:", "Add Recipe", "Recipe Name");

            if (!string.IsNullOrEmpty(recipeName))
            {
                newRecipe.Name = recipeName; // Set the name of the new recipe

                bool addIngredients = true;
                while (addIngredients)
                {
                    // Prompt user to enter ingredient details
                    string ingredientName = Microsoft.VisualBasic.Interaction.InputBox("Enter Ingredient Name:", "Add Ingredient", "Ingredient Name");
                    if (string.IsNullOrEmpty(ingredientName)) break; // Exit loop if ingredient name is empty

                    // Prompt user to enter quantity (assume valid integer input)
                    if (!int.TryParse(Microsoft.VisualBasic.Interaction.InputBox("Enter Quantity:", "Add Ingredient", "1"), out int quantity)) break;

                    // Prompt user to enter calories (assume valid integer input)
                    if (!int.TryParse(Microsoft.VisualBasic.Interaction.InputBox("Enter Calories:", "Add Ingredient", "100"), out int calories)) break;

                    // Prompt user to enter food group
                    string foodGroup = Microsoft.VisualBasic.Interaction.InputBox("Enter Food Group:", "Add Ingredient", "Food Group");

                    // Create a new Ingredient object and add it to the new recipe's Ingredients list
                    newRecipe.Ingredients.Add(new Ingredient(ingredientName, quantity, calories, foodGroup));

                    // Ask user if they want to add another ingredient
                    addIngredients = MessageBox.Show("Add another ingredient?", "Add Ingredient", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                }

                // Add the new recipe to the RecipeManager and refresh the ListBox
                recipeManager.AddRecipe(newRecipe);
                RecipesListBox.Items.Refresh();
            }
        }

        // Event handler for Filter button click
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string filterText = FilterTextBox.Text; // Get the text entered in FilterTextBox

            // Filter recipes based on ingredient name containing filterText
            RecipesListBox.ItemsSource = recipeManager.FilterRecipes(filterText);
        }

        // Event handler for selection change in RecipesListBox
        private void RecipesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Check if a recipe is selected in RecipesListBox
            if (RecipesListBox.SelectedItem is Recipe selectedRecipe)
            {
                // Display the details of the selected recipe in RecipeDetailsTextBlock
                RecipeDetailsTextBlock.Text = $"Recipe: {selectedRecipe.Name}\nIngredients:\n" +
                                              string.Join("\n", selectedRecipe.Ingredients.Select(i => $"{i.Name}: {i.Quantity} {i.FoodGroup}, Calories: {i.Calories}"));
            }
        }
    }
}
