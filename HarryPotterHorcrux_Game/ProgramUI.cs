using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HarryPotter_Game
{
    public enum DeathlyHallow { InvisibilityCloak, ElderWand, ResurrectionStone };

    public class ProgramUI
    {
        // Consider implemeting a timer when games starts to collect all items, otherwise you LOSE

        public List<DeathlyHallow> collectedHallows = new List<DeathlyHallow>();

        Dictionary<string, Room> Rooms = new Dictionary<string, Room>
        {
            { "dormitory", dormitory },
            { "dorm", dormitory },
            { "great hall", greatHall },
            { "dungeon", dungeon },
            { "potions", potions },
            { "grounds", grounds },
            { "hagrids", hagrids },
            { "forest", forest }
        };


        // DORM: gather invisibility cloak. Exit: GREAT HALL
        public static Room dormitory = new Room("You are in your dormitory. Your bed looks safe and comfy,\n" +
            "but the shouts of scared students in the castle awaken your courage.\n\n" +
            "Obvious exit is GREAT HALL\n",
            new List<string> { "great hall" },
            new List<DeathlyHallow> { DeathlyHallow.InvisibilityCloak });

        // GREAT HALL: Exit: GROUNDS or DUNGEON
        public static Room greatHall = new Room("\n\n\nThe Great Hall is filled with scared students \n" +
            "and franctic teachers. You must hurry!\n\n" +
            "Obvious exits are GROUNDS and DUNGEON\n",
            new List<string> { "grounds", "dungeon" },
            new List<DeathlyHallow> { });

        // DUNGEON: Exit: GREAT HALL or POTIONS
        public static Room dungeon = new Room("\n\n\nThe dungeon is crawling with spineless Slytherins. They are no help.\n" +
            "You ignore them and keep looking for the Deathly Hallows.\n\n" +
            "Obvious exits are GREAT HALL or POTIONS classroom.\n",
            new List<string> { "great hall", "potions" },
            new List<DeathlyHallow> { });

        // POTIONS: gather elder wand. Exit: DUNGEON
        public static Room potions = new Room("\n\n\nThe Potions classroom smells of burnt potions made by first years.\n\n" +
            "Obvious exit is DUNGEON\n",
            new List<string> { "dungeon" },
            new List<DeathlyHallow> { DeathlyHallow.ElderWand });

        // GROUNDS: Exit: GREAT HALL or HAGRIDS
        public static Room grounds = new Room("\n\n\nThe Grounds look like a war zone. Even the Whomping Willow gives a weary shudder.\n" +
            "You are surprised to see smoke puffing from the chimney \n" +
            "of Hagrid's hut down by the Forbidden Forest.\n\n" +
            "Obvious exits are HAGRIDS or GREAT HALL\n",
            new List<string> { "great hall", "hagrids" },
            new List<DeathlyHallow> { });

        // HAGRIDS: gather resurrection stone. Exit: GROUNDS or (if hasAllHallows = true) **FORBIDDEN FOREST**
        public static Room hagrids = new Room("\n\n\nHagrid sits in the corner of his tiny hut, looking defeated.\n\n" +
            "HAGRID: Harry, You-Know-Who is hiding in the Forbidden Forest. \n" +
            "        Do you have all the DEATHLY HALLOWS?\n\n" +
            "Obvious exits are GROUNDS or FORBIDDEN FOREST\n",
            new List<string> { "grounds", "forest" },
            new List<DeathlyHallow> { DeathlyHallow.ResurrectionStone });

        // FORBIDDEN FOREST: ** only unlocked if hasAllHallows = true; Cannot escape. Must dice duel with Voldy
        public static Room forest = new Room("\n\n\nThe Forbidden Forest...\n" +
            "...Shrieks and howls sound in the distance...\n" +
            "...Whispers from shadows between the trees...\n" +
            "...\n" +
            "VOLDEMORT appears!\n\n",
            new List<string> { },
            new List<DeathlyHallow> { });



        public void Run()
        {
            Room currentRoom = dormitory;

            Console.WriteLine("HARRY POTTER and the SEARCH FOR DEATHLY HALLOWS");

            Thread.Sleep(1200);

            Console.WriteLine("You wake up in your dormitory to sounds of screams in the castle, \n" +
                "and something small tugging at the hem of your robes...");

            Thread.Sleep(1000);

            Console.WriteLine("\nDOBBY: Harry Potter, sir, your friends need your help!\n" +
                "       Take your invisibility cloak.\n" +
                "       You must gather the remaining two DEATHLY HALLOWS from the castle\n" +
                "       before you can defeat He-Who-Must-Not-Be-Named!");

            Thread.Sleep(1250);

            Console.WriteLine("\n\nHOW TO PLAY:\n" +
                "Type your desired destination from the list of possible exits to move about the castle\n" +
                "Collect the remaining DEATHLY HALLOWS to battle Voldemort\n" +
                "Type \"inventory\" at any time to access INVENTORY\n\n\n" +
                "Press any key to begin...");

            Console.ReadKey();

            Console.Clear();


            currentRoom.TakeHallow(DeathlyHallow.InvisibilityCloak);
            collectedHallows.Add(DeathlyHallow.InvisibilityCloak);


            bool exploring = true;
            while (exploring)
            {
                Thread.Sleep(250);
                Console.WriteLine(currentRoom.Description);

                Console.WriteLine("Where would you like to go?\n");

                string command = Console.ReadLine().ToLower();
                //Console.Clear();


                bool canExit = false;
                if (command.Contains("inventory"))
                {
                    foreach (DeathlyHallow hallow in collectedHallows)
                    {
                        Console.WriteLine("- " + hallow);
                    }
                    Console.WriteLine("\nPress any key to continue");
                    Console.ReadKey();
                }
                else
                {
                    foreach (string exit in currentRoom.Exits)
                    {
                        if (command.Contains("forest") && collectedHallows.Count < 3)
                        {
                            Console.WriteLine("\nI can't go to the Forbidden Forest yet.\n" +
                                "I'll need all three Deathly Hallows to defeat Lord Voldemort.");
                            Console.ReadKey();
                        }


                        else if (command.Contains(exit) && Rooms.ContainsKey(exit))
                        {
                            currentRoom = Rooms[exit];
                            if (currentRoom == forest)
                            {
                                exploring = false;
                            }

                            bool foundHallow = false;
                            foreach (DeathlyHallow hallow in currentRoom.DeathlyHallows)
                            {
                                if (!foundHallow)
                                {
                                    Console.WriteLine($"\n\n*******The {hallow}!*******\n" +
                                        $"You found a DEATHLY HALLOW! Keep it safe.\n\n" +
                                        $"Press any key to keep going...\n");
                                    currentRoom.TakeHallow(hallow);
                                    collectedHallows.Add(hallow);
                                    foundHallow = true;
                                    Console.ReadKey();
                                    break;
                                }
                            }
                            canExit = true;
                            break;
                        }
                    }
                    if (!canExit)
                    {
                        Console.WriteLine("\nI can't go there.");
                        Thread.Sleep(250);
                    }
                }
            }


            Console.WriteLine("\n\n\nThe Forbidden Forest...\n" +
            "...Shrieks and howls sound in the distance...\n" +
            "...Whispers from shadows between the trees...\n" +
            "...\n" +
            "VOLDEMORT appears!\n\n");
            Console.WriteLine("You must duel with Voldemort for five rounds!\n");
            Console.WriteLine("Press any key to begin battle...\n");
            Console.ReadKey();

            bool alive = true;
            while (alive)
            {
                int Harry;
                int Voldemort;

                int harryPoints = 0;
                int voldyPoints = 0;

                Random randy = new Random();

                for (int i = 0; i < 5; i++)
                {

                    Console.WriteLine("Press any key to attack");

                    Console.ReadKey();

                    Harry = randy.Next(1, 7);
                    Voldemort = randy.Next(1, 7);


                    Thread.Sleep(1000);

                    if (Harry > Voldemort)
                    {
                        harryPoints++;
                        Console.WriteLine("You hit Voldemort with Expelliarmus!");

                    }
                    else if (Harry < Voldemort)
                    {
                        voldyPoints++;
                        Console.WriteLine("Voldemort attacks swiftly with Crucio");
                    }
                    else
                    {
                        Console.WriteLine("Your spells rebounded off each other!");
                    }

                    Console.WriteLine($"SCORE - Harry: {harryPoints}     Voldemort: {voldyPoints}\n");
                }

                if (harryPoints > voldyPoints)
                {
                    Console.WriteLine("You've defeated Voldemort!\n" +
                        "Finally, the wizarding world can live in peace...\n\n" +
                        "Thanks to The Boy Who Lived");
                }
                else if (harryPoints < voldyPoints)
                {
                    Console.WriteLine("Voldemort has won...");
                }
                else
                {
                    Console.WriteLine("You and Voldemort are equally matched in power,\n" +
                        "but he can never truly succeed without the love and support you\n" +
                        "have from your friends.\n\n" +
                        "You shall one day duel again...\n");
                }
                Console.ReadKey();
                alive = false;
            }
        }
    }
}
