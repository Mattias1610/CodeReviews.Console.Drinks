﻿using DrinksInfo.Models;
using Newtonsoft.Json;
using RestSharp;
using Spectre.Console;

namespace DrinksInfo;

internal class DataAccess
{
    public static void SearchByCategories(string category)
    {
        var client = new RestClient("https://www.thecocktaildb.com/api/json/v1/1/");
        var request = new RestRequest($"filter.php?c={category}");
        var response = client.Execute(request);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            string responseJson = response.Content;
            var serialize = JsonConvert.DeserializeObject<DrinksModel>(responseJson);

            List<drinks> drinkslist = serialize.drinks;

            var drinkchoice = AnsiConsole.Prompt(new SelectionPrompt<drinks>().Title("select one drink").AddChoices(drinkslist));

            GetInfo(drinkchoice.ToString());

            Console.ReadKey();
        }
    }

    public static void GetInfo(string name)
    {
        var client1 = new RestClient("https://www.thecocktaildb.com/api/json/v1/1/");
        var request1 = new RestRequest($"search.php?s={name}");
        var response1 = client1.Execute(request1);
        if (response1.StatusCode == System.Net.HttpStatusCode.OK)
        {
            string responseJson1 = response1.Content;
            var serialize1 = JsonConvert.DeserializeObject<DrinksModel>(responseJson1);

            List<drinks> returned = serialize1.drinks;
            drinks selectedDrink = returned.FirstOrDefault();

            UserInterface.PrintDrink(selectedDrink);
        }
    }

    public static void RandomDrink()
    {
        var client = new RestClient("https://www.thecocktaildb.com/api/json/v1/1/");
        var request = new RestRequest("random.php");
        var response = client.Execute(request);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            string responseJson = response.Content;
            var serialize = JsonConvert.DeserializeObject<DrinksModel>(responseJson);

            List<drinks> returned = serialize.drinks;
            drinks returnedRandom = returned.FirstOrDefault();

            UserInterface.PrintDrink(returnedRandom);
        }
    }

    public static void SearchByName(string name)
    {
        var client = new RestClient("https://www.thecocktaildb.com/api/json/v1/1/");
        var request = new RestRequest($"search.php?s={name}");
        var response = client.Execute(request);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            string responseJson = response.Content;
            var serialize = JsonConvert.DeserializeObject<DrinksModel>(responseJson);

            List<drinks> returnedList = serialize.drinks;

            if (returnedList != null)
            {
                var drinkchoice = AnsiConsole.Prompt(new SelectionPrompt<drinks>().Title("select one drink").AddChoices(returnedList));
                GetInfo(drinkchoice.ToString());
            }
            else
            {
                Console.WriteLine("No results found");
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
    }
}
