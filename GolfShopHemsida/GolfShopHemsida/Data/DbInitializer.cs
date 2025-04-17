using GolfShopHemsida.Models;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Items.Any())
        {
            return; // DB has been seeded
        }



        // Add products
        var items = new[]
        {
            // Golf sets
            new Item {
                ItemId = "1",
                Name = "Golf-set Junior",
                Price = 749,
                Stock = 10,
                Description = "Ett komplett golfset för juniorer, inklusive klubbor och väska.",
                ImageUrl = "/Images/Shop/Golfset-barn.png"

            },
            new Item {
                ItemId = "2",
                Name = "Callaway-XL",
                Price = 3400,
                Stock = 3,
                Description = "Ett komplett golfset för proffs, inklusive klubbor och väska.",
                ImageUrl = "./Images/Shop/Callaway-golfset.png"

            },
            new Item {
                ItemId = "3",
                Name = "X LZR Womens",
                Price = 2200,
                Stock = 7,
                Description = "Ett komplett golfset anpassat för kvinnor, inklusive klubbor och väska.",
                ImageUrl = "./Images/Shop/X-LZR-womens-set.jpg"

            },
            new Item {
                ItemId = "4",
                Name = "Callaway-Pro",
                Price = 5499,
                Stock = 10,
                Description = "Ett komplett golfset för absoluta sigma golf proffs, inklusive klubbor och väska.",
                ImageUrl = "./Images/Shop/Callaway-Pro-mens.jpg"

            },
            // Klubbor
            new Item {
                ItemId = "5",
                Name = "Taylor Made Stealth 2",
                Price = 2500,
                Stock = 10,
                Description = "En driver med avancerad teknik för ökad precision och längd.",
                ImageUrl = "./Images/Shop/Drivers.png"

            },
            new Item {
                ItemId = "6",
                Name = "Cobra Darkspeed X",
                Price = 2000,
                Stock = 10,
                Description = "En Fairway woods klubba med lättviktsdesign för snabbare sving och längre slag.",
                ImageUrl = "./Images/Shop/FAIRWAY-WOODS.png"

            },
            new Item {
                ItemId = "7",
                Name = "TaylorMade Spidertour X",
                Price = 1900,
                Stock = 10,
                Description = "En putter med en unik design för bättre balans och kontroll.",
                ImageUrl = "./Images/Shop/PUTTER.png",

            },
            new Item {
                ItemId = "8",
                Name = "Cleveland RTX6",
                Price = 2700,
                Stock = 10,
                Description = "En wedge med avancerad teknologi för bättre grepp och kontroll.",
                ImageUrl = "./Images/Shop/WEDGAR.jpg",

            },
            // Bollar
            new Item {
                ItemId = "9",
                Name = "Titleist Pro V1 12-pack",
                Price = 600,
                Stock = 10,
                Description = "En högpresterande golfboll med utmärkt känsla och kontroll.",
                ImageUrl = "./Images/Shop/titleist.jpg"

            },
            new Item {
                ItemId = "10",
                Name = "Callaway 12-pack",
                Price = 550,
                Stock = 10,
                Description = "En högpresterande golfboll med utmärkt känsla och kontroll.",
                ImageUrl = "./Images/Shop/callaway.png",

            },
            new Item {
                ItemId = "11",
                Name = "Pinnacle Rush 12-pack",
                Price = 400,
                Stock = 10,
                Description = "Ett komplett golfset för juniorer, inklusive klubbor och väska.",
                ImageUrl = "./Images/Shop/pinnacle.jpg"

            },
            new Item {
                ItemId = "12",
                Name = "Srixon Soft Feel 12-pack",
                Price = 500,
                Stock = 10,
                Description = "En högpresterande golfboll med utmärkt känsla och kontroll.",
                ImageUrl = "./Images/Shop/bridgestone.jpg"
            },
            // Tillbehör
            new Item {
                ItemId = "13",
                Name = "Golfhandskar",
                Price = 200,
                Stock = 10,
                Description = "Ett par högkvalitativa golfhandskar för bättre grepp och komfort.",
                ImageUrl = "./Images/Shop/handskar.png"
                },
            new Item {
                ItemId = "14",
                Name = "Golf T-shirt 4-pack",
                Price = 50,
                Stock = 10,
                Description = "Ett stilrent golfbälte för att hålla byxorna på plats.",
                ImageUrl = "./Images/Shop/tees.jpg"
            },
            new Item {
                ItemId = "15",
                Name = "Golfbag",
                Price = 1250,
                Stock = 10,
                Description = "En lätt och hållbar golfbag för att transportera dina klubbor.",
                ImageUrl = "./Images/Shop/bag.jpg"
                },
            new Item {
                ItemId = "16",
                Name = "Avståndsmätare",
                Price = 3500,
                Stock = 10,
                Description = "En avancerad avståndsmätare för att mäta avståndet till flaggan.",
                ImageUrl = "./Images/Shop/mätare.jpg"
            },





        };

        context.Items.AddRange(items);
        context.SaveChanges();


    }
}