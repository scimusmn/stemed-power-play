using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
    public static int energyScore = 100;
    public static int timeScore = 100;
    public static int overallScore = 0;
    public static int scenario = 11;
    public static readonly int maxEnergyScore = 100;
    public static readonly int maxTimeScore = 100;
    public static readonly int maxOverallScore = 260;
    public static readonly int scorePerTurn = 10;
    public static int startGameSceneNum = 11;
    public static int endGameSceneNum = 23;

    // Scenarios, not necessarily in order.
    public static Scenario[] scenarios = new Scenario[]
    {
        new Scenario(0, new Vector3(0f, -0.5f, -10f), 4f, new int[]{ 0, 1, 2, 3 },
            new DisabledChoice[]{ new DisabledChoice(2, 0.5f, "Tech Fail: Car is broken down!", false, 999),
            new DisabledChoice(0, 1f, "Tech Fail: It's raining, so you can't walk!", true, 0) }, (999, 0f, ""), "You have to go to work. How should you get there?"),
        new Scenario(1, new Vector3(-12f, -2f, -10f), 5f, new int[]{ 4 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Let's get to work!"),
        new Scenario(2, new Vector3(0f, 2f, -10f), 7f, new int[]{ 0, 1, 7 },
            new DisabledChoice[]{ }, (7, 0.5f, "New Tech: You helped invent a pneumatic tube system!"), "Should you take the stairs or the elevator to your office?"),
        new Scenario(3, new Vector3(1f, 2.5f, -10f), 4.5f, new int[]{ 2, 3 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Lunch time! Should you make it yourself or go out to eat?"),
        new Scenario(4, new Vector3(-6.5f, 3.25f, -10f), 4f, new int[]{ 4, 5 },
            new DisabledChoice[]{ new DisabledChoice(4, 0.5f, "Tech Fail: Someone put gum in the fountain!", false, 999) },
            (999, 0f, ""), "You look thirsty. Should you go to the water fountain or buy a bottle?"),
        new Scenario(5, new Vector3(-3f, 0f, -10f), 7f, new int[]{ 6 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Time to head home!"),
        new Scenario(6, new Vector3(-5f, 2f, -10f), 6f, new int[]{ 2, 3 },
            new DisabledChoice[]{ }, (999, 0f, ""), "What a day! Order burgers or cook a healthy dinner?"),
        new Scenario(7, new Vector3(-3f, 2f, -10f), 6f, new int[]{ 4, 5 },
            new DisabledChoice[]{ new DisabledChoice(5, 0.5f, "Tech Fail: Your washer sprung a leak!", false, 999) }, 
            (999, 0f, ""), "Wear your new sweater tomorrow! Run the washer or quick hand wash?"),
        new Scenario(8, new Vector3(-5f, 2f, -10f), 6f, new int[]{ 6, 7 },
            new DisabledChoice[]{ }, (999, 0f, ""), "You want a new copy of 'The Life of Coach Zappa'. Use Amazon or the library?"),
        new Scenario(9, new Vector3(-2f, 2f, -10f), 5f, new int[]{ 8, 9 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Time to rinse off after a great day. Take a bath or shower?"),
        new Scenario(10, new Vector3(-5f, 2f, -10f), 6f, new int[]{ 10 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Good night!"),
        new Scenario(11, new Vector3(-5f, 2f, -10f), 6f, new int[]{ 11 },
            new DisabledChoice[]{ }, (11, 1f, "Don't run out of energy or time! Pick a choice bubble to continue."), "It's dark in here. Let's turn on a light."),
        new Scenario(12, new Vector3(-7f, 2f, -10f), 6f, new int[]{ 0, 1 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Let's make breakfast. Boil water in the microwave or in the kettle?"),
        new Scenario(13, new Vector3(-5f, 2f, -10f), 6f, new int[]{ 12, 13 },
            new DisabledChoice[]{ new DisabledChoice(13, 0.5f, "Tech Fail: Uh oh. The dishwasher is busted.", false, 999) },
            (999, 0f, ""), "Now let's clean up these dishes. Hand wash or use the dishwasher?"),
        new Scenario(14, new Vector3(-5f, 2f, -10f), 6f, new int[]{ 14, 15 },
            new DisabledChoice[]{ }, (999, 0f, ""), "It's cold today. Just leave or adjust the heat down first?"),
        new Scenario(15, new Vector3(-5f, 1f, -10f), 6f, new int[]{ 16, 17 },
            new DisabledChoice[]{ }, (16, 1f, "New Tech: You hooked up all your electronics to a power strip!"),
            "Leaving chargers plugged in uses energy. Unplug chargers or just leave?"),
        new Scenario(16, new Vector3(-5f, 2f, -10f), 6f, new int[]{ 19 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Time to get to work!"),
        new Scenario(17, new Vector3(-5f, 2f, -10f), 6f, new int[]{ 18 },
            new DisabledChoice[]{ }, (999, 0f, ""), "You made it through day 1! Let's head out and do something fun."),
        new Scenario(18, new Vector3(0f, -0.5f, -10f), 4f, new int[]{ 5, 6 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Where should we go today?"),
        new Scenario(19, new Vector3(-14f, 9f, -10f), 6f, new int[]{ 8 },
            new DisabledChoice[]{ }, (999, 0f, ""), "All right! Let's go to Valleyfair!"),
        new Scenario(20, new Vector3(11f, -7f, -10f), 6f, new int[]{ 7 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Woot! Let's go to the museum!"),
        new Scenario(21, new Vector3(-13.5f, 9f, -10f), 6f, new int[]{ 9, 10 },
            new DisabledChoice[]{ new DisabledChoice(9, 0.5f, "Tech Fail: The arcade's power is out!", false, 999) },
            (999, 0f, ""), "Play video games or old-school carnival games with friends?"),
        new Scenario(22, new Vector3(-14f, 11f, -10f), 5f, new int[]{ 11, 12 },
            new DisabledChoice[]{ }, (999, 0f, ""), "That was fun! Eat a picnic lunch or buy Valleyfair food?"),
        new Scenario(23, new Vector3(0f, 0f, -10f), 4f, new int[]{ },
            new DisabledChoice[]{ }, (999, 0f, ""), "Wow! You balanced your energy and your time and made it to the end!"),
        new Scenario(24, new Vector3(0f, 0f, -10f), 4f, new int[]{ 14, 15, 16, 17 },
            new DisabledChoice[]{ }, (15, 1f, "Tech Advocate: You helped lobby to extend the bus system!"), "How do you want to get to Valleyfair?"),
        new Scenario(25, new Vector3(0f, 0f, -10f), 4f, new int[]{ 18, 19, 20, 21 },
            new DisabledChoice[]{ }, (999, 0f, ""), "How do you want to get to the museum?"),
        new Scenario(26, new Vector3(0f, 2f, -10f), 4f, new int[]{ 0, 1 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Let's go to the 5th floor. Take the stairs or the elevator?"),
        new Scenario(27, new Vector3(0f, 2f, -10f), 4f, new int[]{ 2, 3 },
            new DisabledChoice[]{ new DisabledChoice(3, 0.5f, "Tech Fail: Someone jammed up the fountain!", false, 999) },
            (999, 0f, ""), "Out of water again. Buy a bottle or use the water fountain?"),
        new Scenario(28, new Vector3(3f, 6f, -10f), 4f, new int[]{ 4, 5 },
            new DisabledChoice[]{ new DisabledChoice(5, 0.5f, "Awesome Advocacy: You organized to shut down the old power plant!", false, 999) }, 
            (999, 0f, ""), "Should the museum use renewable or thermal energy for its electricity?"),
        new Scenario(29, new Vector3(-8f, 4f, -10f), 4f, new int[]{ 6, 7 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Check out the theater or go to the dinosaur exhibit?"),
        new Scenario(30, new Vector3(-4f, 9f, -10f), 5f, new int[]{ 22, 23 },
            new DisabledChoice[]{ }, (999, 0f, ""), "Should Valleyfair use renewable or thermal energy for its electricity?")
    };
}
