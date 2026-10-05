using System.Net.Http.Json;
using System.Text.Json;

while (true)

{
    Console.WriteLine("=== ONLINE SCOREBOARD ===");
    Console.WriteLine();
    Console.WriteLine("1. Submit Score");
    Console.WriteLine("2. View Scoreboard");
    Console.WriteLine("3. Exit");
    Console.WriteLine();
    Console.Write("Choose: ");

    string? choice = Console.ReadLine();

    if (choice == "1")
    {
        Console.Write("Enter your name: ");
        string? name = Console.ReadLine();

        Console.Write("Enter your score: ");
        string? scoreInput = Console.ReadLine();

        if (int.TryParse(scoreInput, out int score))
        {
            using HttpClient client = new HttpClient();

            var scoreEntry = new ScoreEntry
            {
                Name = name ?? "",
                Score = score
            };

            string url = "https://hooks.zapier.com/hooks/catch/8338993/ujs9jj9/";

            try
            {
                HttpResponseMessage response =
                    await client.PostAsJsonAsync(url, scoreEntry);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Score submitted!");
                }
                else
                {
                    Console.WriteLine("Failed to submit score.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Network error. Could not submit score.");
            }
        }
        else
        {
            Console.WriteLine("Invalid score.");
        }
    }
    else if (choice == "2")
    {
        using HttpClient client = new HttpClient();

        string url = "https://script.google.com/macros/s/AKfycbys5aEPMvNCutyhNYYCcQcCjzsi2UtqNspmKyCH-AicJxJbCJMrAoT0LUaYaXhTWA8n/exec";
    
        try
        {
            string json = await client.GetStringAsync(url);
            json = json.Replace("\"name\":2", "\"name\":\"2\"");

            List<ScoreEntry>? scores =
                JsonSerializer.Deserialize<List<ScoreEntry>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (scores != null)
            {
                foreach (ScoreEntry entry in scores.OrderByDescending(s => s.Score))
                {
                    Console.WriteLine($"{entry.Name}: {entry.Score}");
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Network error. Could not load scoreboard.");
        }
    }
    else if (choice == "3")
    {
        break;
    }
    else
    {
        Console.WriteLine("Invalid choice.");
    }

    Console.WriteLine();
}