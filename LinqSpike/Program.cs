// ******************************************************************************************************************
//  LINQ Spike - simple LINQ expressions which query a local DB.
//  Copyright(C) 2018  James LoForti
//  Contact Info: jamesloforti@gmail.com
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.If not, see<https://www.gnu.org/licenses/>.
//									     ____.           .____             _____  _______   
//									    |    |           |    |    ____   /  |  | \   _  \  
//									    |    |   ______  |    |   /  _ \ /   |  |_/  /_\  \ 
//									/\__|    |  /_____/  |    |__(  <_> )    ^   /\  \_/   \
//									\________|           |_______ \____/\____   |  \_____  /
//									                             \/          |__|        \/ 
//
// ******************************************************************************************************************
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSpike
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declare Constants:
            const string ABOUT = "Name: James LoForti \n*************************************\n ";
            const string RESULTS_HEADER = "************Query Results************ \n";
            const string QUERY0_HEADER = "\n\tQuery 0 Results: ";
            const string QUERY1_HEADER = "\n\tQuery 1 Results: ";
            const string QUERY2_HEADER = "\n\tQuery 2 Results: ";
            const string QUERY3_HEADER = "\n\tQuery 3 Results: ";
            const string QUERY4_HEADER = "\n\tQuery 4 Results: ";
            const string QUERY5_HEADER = "\n\tQuery 5 Results: ";
            const int THREE_NUM = 3;
            const int TWO_NUM = 2;
            const int HUNDRED_NUM = 100;
            const int FIVE_HUNDRED_NUM = 500;
            const int TEN_NUM = 10;
            const int SIXTY_SIX_NUM = 66;

            //Declare & Initialize Variables:
            int[] odds = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
            int[] evens = { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18 };
            var data = new[]
            {
                new { ID = 100, Name = "Grumpy", Job = "Miner" },
                new { ID = 100, Name = "Sleepy", Job = "Mattress Salesman" },
                new { ID = 200, Name = "Snizzy", Job = "Allergist" },
                new { ID = 300, Name = "Doc", Job = "M.D." },
                new { ID = 400, Name = "Happy", Job = "Professor" },
                new { ID = 500, Name = "Dopey", Job = "Student" },
                new { ID = 600, Name = "Bashful", Job = "Programmer" },
                new { ID = 220, Name = "Snow White", Job = "Princess" },
                new { ID = 150, Name = "Prince Charming", Job = "Prince" },
                new { ID = 175, Name = "Wicked Witch", Job = "Queen" }
            }; // end init data

            //Print headers
            Console.WriteLine(ABOUT);
            Console.WriteLine(RESULTS_HEADER);

            //Query0 – Find the values in odds that are divisible (no remainder) by three.
            Console.WriteLine(QUERY0_HEADER); // print header
            List<int> results0 = odds.Where(x => (x % THREE_NUM).Equals(0)).ToList(); // LINQ
            results0.ForEach(i => Console.WriteLine("\t\t{0}", i)); // print results

            //Query1 – Find the values in evens and the values in odds 
            //where the even values are greater than 2x the odd values.  
            Console.WriteLine(QUERY1_HEADER); // print header
            var results1 =
                from o in odds
                from e in evens
                where e >= (TWO_NUM * o)
                select new { odd = o, even = e }; // LINQ 
            // print results
            foreach (var result in results1)
            {
                Console.WriteLine($"\t\t{result.odd}\t{result.even}");
            } // end foreach

            //Query2 – Find values in data where the ID is greater than 100 and less than 500 and the Name contains a ‘y’ 
            //or Job ends with ‘e’ or Job contains ‘e’ or Job equals “Student” or Job equals “Programmer”.
            Console.WriteLine(QUERY2_HEADER); // print header
            var results2 = data.Where(x =>
                x.ID > HUNDRED_NUM &&
                x.ID < FIVE_HUNDRED_NUM &&
                x.Name.Contains("y") ||
                x.Job.EndsWith("e") ||
                x.Job.Contains("e") ||
                x.Job.Equals("Student") ||
                x.Job.Equals("Programmer")).ToList(); // LINQ
            results2.ForEach(i => Console.WriteLine($"\t\t{i}")); // print results

            //Query3 – Find values in productList where the ProductID is greater than 10 and less than 66, order in 
            //descending order by UnitsInStock and write the ProductName and UnitsInStock to the Console.
            Console.WriteLine(QUERY3_HEADER); // print header
            var results3 = GetProductList().Where(x => 
                x.ProductID > TEN_NUM && 
                x.ProductID < SIXTY_SIX_NUM).
                OrderByDescending(x => x.UnitsInStock).
                ToList(); // LINQ
            results3.ForEach(i => Console.WriteLine($"\t\t{i.ProductName} \t{i.UnitsInStock}")); // print results

            var myList = GetProductList();

            //Query4 – Write a complex query on the productList that shows you know how to use at least ten(10) different 
            //LINQ operations i.e.let, where, orderby, into, from, etc.
            Console.WriteLine(QUERY4_HEADER); // print header
            var results4 = GetProductList().
                OrderByDescending(p => p.UnitPrice).
                ThenBy(x => x.ProductName).
                Take(77).
                Where(x => x.UnitPrice > TEN_NUM).
                TakeWhile(x => x.UnitsInStock > 5).
                SkipWhile(x => x.Category == "Confections").
                Select(x => x).
                Reverse().
                Distinct().
                ToList(); // LINQ
            results4.ForEach(i => Console.WriteLine($"\t\t{i.ProductName}\t{i.UnitPrice:C}\t{i.UnitsInStock}")); // print results

            //Query5 – Write a query on a table in the SQL Server database that demonstrates 
            //you understand how to work with the SQL Server database.
            Console.WriteLine(QUERY5_HEADER); // print header
            //Get a reference to the items table
            DataClassesItemsDataContext itemsDC = new DataClassesItemsDataContext();
            var results5 =
                from item in itemsDC.Items
                where item.ItemPrice > 4
                where item.ItemName.Contains("o")
                select item;

            // print results
            foreach (var result in results5)
            {
                Console.WriteLine($"\t\t{result.ItemName}\t{result.ItemCategory}");
            } // end foreach

            //Halt for user input, then exit
            Console.Write("Press any key to continue... ");
            Console.ReadKey(true);
        } // end method Main()

        /// <summary>
        /// To initialize a list of products, and return it
        /// </summary>
        public static List<Product> GetProductList()
        {
            //Initialize list of products and save as productList
            List<Product> productList = new List<Product>
            {
                new Product{ ProductID = 1, ProductName = "Chai", Category = "Beverages",
                    UnitPrice = 18.0000M, UnitsInStock = 39 },
                new Product{ ProductID = 2, ProductName = "Chang", Category = "Beverages",
                    UnitPrice = 19.0000M, UnitsInStock = 17 },
                new Product{ ProductID = 3, ProductName = "Aniseed Syrup", Category = "Condiments",
                    UnitPrice = 10.0000M, UnitsInStock = 13 },
                new Product{ ProductID = 4, ProductName = "Chef Anton's Cajun Seasoning", Category = "Condiments",
                    UnitPrice = 22.0000M, UnitsInStock = 53 },
                new Product{ ProductID = 5, ProductName = "Chef Anton's Gumbo Mix", Category = "Condiments",
                    UnitPrice = 21.3500M, UnitsInStock = 0 },
                new Product{ ProductID = 6, ProductName = "Grandma's Boysenberry Spread", Category = "Condiments",
                    UnitPrice = 25.0000M, UnitsInStock = 120 },
                new Product{ ProductID = 7, ProductName = "Uncle Bob's Organic Dried Pears", Category = "Produce",
                    UnitPrice = 30.0000M, UnitsInStock = 15 },
                new Product{ ProductID = 8, ProductName = "Northwoods Cranberry Sauce", Category = "Condiments",
                    UnitPrice = 40.0000M, UnitsInStock = 6 },
                new Product{ ProductID = 9, ProductName = "Mishi Kobe Niku", Category = "Meat/Poultry",
                    UnitPrice = 97.0000M, UnitsInStock = 29 },
                new Product{ ProductID = 10, ProductName = "Ikura", Category = "Seafood",
                    UnitPrice = 31.0000M, UnitsInStock = 31 },
                new Product{ ProductID = 11, ProductName = "Queso Cabrales", Category = "Dairy Products",
                    UnitPrice = 21.0000M, UnitsInStock = 22 },
                new Product{ ProductID = 12, ProductName = "Queso Manchego La Pastora", Category = "Dairy Products",
                    UnitPrice = 38.0000M, UnitsInStock = 86 },
                new Product{ ProductID = 13, ProductName = "Konbu", Category = "Seafood",
                    UnitPrice = 6.0000M, UnitsInStock = 24 },
                new Product{ ProductID = 14, ProductName = "Tofu", Category = "Produce",
                    UnitPrice = 23.2500M, UnitsInStock = 35 },
                new Product{ ProductID = 15, ProductName = "Genen Shouyu", Category = "Condiments",
                    UnitPrice = 15.5000M, UnitsInStock = 39 },
                new Product{ ProductID = 16, ProductName = "Pavlova", Category = "Confections",
                    UnitPrice = 17.4500M, UnitsInStock = 29 },
                new Product{ ProductID = 17, ProductName = "Alice Mutton", Category = "Meat/Poultry",
                    UnitPrice = 39.0000M, UnitsInStock = 0 },
                new Product{ ProductID = 18, ProductName = "Carnarvon Tigers", Category = "Seafood",
                    UnitPrice = 62.5000M, UnitsInStock = 42 },
                new Product{ ProductID = 19, ProductName = "Teatime Chocolate Biscuits", Category = "Confections",
                    UnitPrice = 9.2000M, UnitsInStock = 25 },
                new Product{ ProductID = 20, ProductName = "Sir Rodney's Marmalade", Category = "Confections",
                    UnitPrice = 81.0000M, UnitsInStock = 40 },
                new Product{ ProductID = 21, ProductName = "Sir Rodney's Scones", Category = "Confections",
                    UnitPrice = 10.0000M, UnitsInStock = 3 },
                new Product{ ProductID = 22, ProductName = "Gustaf's Knäckebröd", Category = "Grains/Cereals",
                    UnitPrice = 21.0000M, UnitsInStock = 104 },
                new Product{ ProductID = 23, ProductName = "Tunnbröd", Category = "Grains/Cereals",
                    UnitPrice = 9.0000M, UnitsInStock = 61 },
                new Product{ ProductID = 24, ProductName = "Guaraná Fantástica", Category = "Beverages",
                    UnitPrice = 4.5000M, UnitsInStock = 20 },
                new Product{ ProductID = 25, ProductName = "NuNuCa Nuß-Nougat-Creme", Category = "Confections",
                    UnitPrice = 14.0000M, UnitsInStock = 76 },
                new Product{ ProductID = 26, ProductName = "Gumbär Gummibärchen", Category = "Confections",
                    UnitPrice = 31.2300M, UnitsInStock = 15 },
                new Product{ ProductID = 27, ProductName = "Schoggi Schokolade", Category = "Confections",
                    UnitPrice = 43.9000M, UnitsInStock = 49 },
                new Product{ ProductID = 28, ProductName = "Rössle Sauerkraut", Category = "Produce",
                    UnitPrice = 45.6000M, UnitsInStock = 26 },
                new Product{ ProductID = 29, ProductName = "Thüringer Rostbratwurst", Category = "Meat/Poultry",
                    UnitPrice = 123.7900M, UnitsInStock = 0 },
                new Product{ ProductID = 30, ProductName = "Nord-Ost Matjeshering", Category = "Seafood",
                    UnitPrice = 25.8900M, UnitsInStock = 10 },
                new Product{ ProductID = 31, ProductName = "Gorgonzola Telino", Category = "Dairy Products",
                    UnitPrice = 12.5000M, UnitsInStock = 0 },
                new Product{ ProductID = 32, ProductName = "Mascarpone Fabioli", Category = "Dairy Products",
                    UnitPrice = 32.0000M, UnitsInStock = 9 },
                new Product{ ProductID = 33, ProductName = "Geitost", Category = "Dairy Products",
                    UnitPrice = 2.5000M, UnitsInStock = 112 },
                new Product{ ProductID = 34, ProductName = "Sasquatch Ale", Category = "Beverages",
                    UnitPrice = 14.0000M, UnitsInStock = 111 },
                new Product{ ProductID = 35, ProductName = "Steeleye Stout", Category = "Beverages",
                    UnitPrice = 18.0000M, UnitsInStock = 20 },
                new Product{ ProductID = 36, ProductName = "Inlagd Sill", Category = "Seafood",
                    UnitPrice = 19.0000M, UnitsInStock = 112 },
                new Product{ ProductID = 37, ProductName = "Gravad lax", Category = "Seafood",
                    UnitPrice = 26.0000M, UnitsInStock = 11 },
                new Product{ ProductID = 38, ProductName = "Côte de Blaye", Category = "Beverages",
                    UnitPrice = 263.5000M, UnitsInStock = 17 },
                new Product{ ProductID = 39, ProductName = "Chartreuse verte", Category = "Beverages",
                    UnitPrice = 18.0000M, UnitsInStock = 69 },
                new Product{ ProductID = 40, ProductName = "Boston Crab Meat", Category = "Seafood",
                    UnitPrice = 18.4000M, UnitsInStock = 123 },
                new Product{ ProductID = 41, ProductName = "Jack's New England Clam Chowder", Category = "Seafood",
                    UnitPrice = 9.6500M, UnitsInStock = 85 },
                new Product{ ProductID = 42, ProductName = "Singaporean Hokkien Fried Mee", Category = "Grains/Cereals",
                    UnitPrice = 14.0000M, UnitsInStock = 26 },
                new Product{ ProductID = 43, ProductName = "Ipoh Coffee", Category = "Beverages",
                    UnitPrice = 46.0000M, UnitsInStock = 17 },
                new Product{ ProductID = 44, ProductName = "Gula Malacca", Category = "Condiments",
                    UnitPrice = 19.4500M, UnitsInStock = 27 },
                new Product{ ProductID = 45, ProductName = "Rogede sild", Category = "Seafood",
                    UnitPrice = 9.5000M, UnitsInStock = 5 },
                new Product{ ProductID = 46, ProductName = "Spegesild", Category = "Seafood",
                    UnitPrice = 12.0000M, UnitsInStock = 95 },
                new Product{ ProductID = 47, ProductName = "Zaanse koeken", Category = "Confections",
                    UnitPrice = 9.5000M, UnitsInStock = 36 },
                new Product{ ProductID = 48, ProductName = "Chocolade", Category = "Confections",
                    UnitPrice = 12.7500M, UnitsInStock = 15 },
                new Product{ ProductID = 49, ProductName = "Maxilaku", Category = "Confections",
                    UnitPrice = 20.0000M, UnitsInStock = 10 },
                new Product{ ProductID = 50, ProductName = "Valkoinen suklaa", Category = "Confections",
                    UnitPrice = 16.2500M, UnitsInStock = 65 },
                new Product{ ProductID = 51, ProductName = "Manjimup Dried Apples", Category = "Produce",
                    UnitPrice = 53.0000M, UnitsInStock = 20 },
                new Product{ ProductID = 52, ProductName = "Filo Mix", Category = "Grains/Cereals",
                    UnitPrice = 7.0000M, UnitsInStock = 38 },
                new Product{ ProductID = 53, ProductName = "Perth Pasties", Category = "Meat/Poultry",
                    UnitPrice = 32.8000M, UnitsInStock = 0 },
                new Product{ ProductID = 54, ProductName = "Tourtière", Category = "Meat/Poultry",
                    UnitPrice = 7.4500M, UnitsInStock = 21 },
                new Product{ ProductID = 55, ProductName = "Pâté chinois", Category = "Meat/Poultry",
                    UnitPrice = 24.0000M, UnitsInStock = 115 },
                new Product{ ProductID = 56, ProductName = "Gnocchi di nonna Alice", Category = "Grains/Cereals",
                    UnitPrice = 38.0000M, UnitsInStock = 21 },
                new Product{ ProductID = 57, ProductName = "Ravioli Angelo", Category = "Grains/Cereals",
                    UnitPrice = 19.5000M, UnitsInStock = 36 },
                new Product{ ProductID = 58, ProductName = "Escargots de Bourgogne", Category = "Seafood",
                    UnitPrice = 13.2500M, UnitsInStock = 62 },
                new Product{ ProductID = 59, ProductName = "Raclette Courdavault", Category = "Dairy Products",
                    UnitPrice = 55.0000M, UnitsInStock = 79 },
                new Product{ ProductID = 60, ProductName = "Camembert Pierrot", Category = "Dairy Products",
                    UnitPrice = 34.0000M, UnitsInStock = 19 },
                new Product{ ProductID = 61, ProductName = "Sirop d'érable", Category = "Condiments",
                    UnitPrice = 28.5000M, UnitsInStock = 113 },
                new Product{ ProductID = 62, ProductName = "Tarte au sucre", Category = "Confections",
                    UnitPrice = 49.3000M, UnitsInStock = 17 },
                new Product{ ProductID = 63, ProductName = "Vegie-spread", Category = "Condiments",
                    UnitPrice = 43.9000M, UnitsInStock = 24 },
                new Product{ ProductID = 64, ProductName = "Wimmers gute Semmelknödel", Category = "Grains/Cereals",
                    UnitPrice = 33.2500M, UnitsInStock = 22 },
                new Product{ ProductID = 65, ProductName = "Louisiana Fiery Hot Pepper Sauce", Category = "Condiments",
                    UnitPrice = 21.0500M, UnitsInStock = 76 },
                new Product{ ProductID = 66, ProductName = "Louisiana Hot Spiced Okra", Category = "Condiments",
                    UnitPrice = 17.0000M, UnitsInStock = 4 },
                new Product{ ProductID = 67, ProductName = "Laughing Lumberjack Lager", Category = "Beverages",
                    UnitPrice = 14.0000M, UnitsInStock = 52 },
                new Product{ ProductID = 68, ProductName = "Scottish Longbreads", Category = "Confections",
                    UnitPrice = 12.5000M, UnitsInStock = 6 },
                new Product{ ProductID = 69, ProductName = "Gudbrandsdalsost", Category = "Dairy Products",
                    UnitPrice = 36.0000M, UnitsInStock = 26 },
                new Product{ ProductID = 70, ProductName = "Outback Lager", Category = "Beverages",
                    UnitPrice = 15.0000M, UnitsInStock = 15 },
                new Product{ ProductID = 71, ProductName = "Flotemysost", Category = "Dairy Products",
                    UnitPrice = 21.5000M, UnitsInStock = 26 },
                new Product{ ProductID = 72, ProductName = "Mozzarella di Giovanni", Category = "Dairy Products",
                    UnitPrice = 34.8000M, UnitsInStock = 14 },
                new Product{ ProductID = 73, ProductName = "Röd Kaviar", Category = "Seafood",
                    UnitPrice = 15.0000M, UnitsInStock = 101 },
                new Product{ ProductID = 74, ProductName = "Longlife Tofu", Category = "Produce",
                    UnitPrice = 10.0000M, UnitsInStock = 4 },
                new Product{ ProductID = 75, ProductName = "Rhönbräu Klosterbier", Category = "Beverages",
                    UnitPrice = 7.7500M, UnitsInStock = 125 },
                new Product{ ProductID = 76, ProductName = "Lakkalikööri", Category = "Beverages",
                    UnitPrice = 18.0000M, UnitsInStock = 57 },
                new Product{ ProductID = 77, ProductName = "Original Frankfurter grüne Soße", Category = "Condiments",
                    UnitPrice = 13.0000M, UnitsInStock = 32 }
            }; // end init productList

            return productList;
        } // end method GetProductList()
    } // end class Program
} // end namespace
