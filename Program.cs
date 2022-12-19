using Isometric.Layout;

class Program {

    public static void Main() {
        // var l = new Layout(20, 20);
        // var game = new IsoGame();
        // game.Run();

        var game = new IsoGame(800, 600);
        game.Run();
    }
}