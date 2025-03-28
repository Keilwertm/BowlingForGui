namespace WPFBowling.Models;

public class BowlingCalculator
{
    public int strike { get; set; }         // All ten pins knocked in the first delivery 
    public int spare { get; set; }          // number of pins remaining after first throw
    public int marks { get; set; }          // the total amount of strike and spares
    public int split { get; set; }          // pins left standing after first delivery 
    public bool doubleStrike { get; set; }  // two strikes in a row
    public bool turkey { get; set; }        // Three strikes in a row
    public bool fourBagger { get; set; }    // four strikes in a row
    public bool foul { get; set; }          // player foul, stepping over line or otherwise   
    public int playerPosition { get; set; }
    public int strikePosition { get; set; }  // user enters a position, strike and gutter are assigned to a random number. Otherwise a random number of pins are knocked over.
    public int gutterPosition { get; set; }
    
    // I know declaring when needed is best practice but this helps my brain put together what is needed as I go through step by step
}

public class roundHandeling
{
    
}