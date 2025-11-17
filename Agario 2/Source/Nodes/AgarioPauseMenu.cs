using MyEngine.Nodes;
using MyEngine.Nodes.UI;
using MyEngine.Timed;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Agario_2.Nodes;

public class AgarioPauseMenu : Node
{
    private static readonly List<TimedSequence<string>.Element> Elements = new()
    {
        (0.4f, "Game is paused."),
        (0.8f, "Game is paused.."),
        (1.2f, "Game is paused..."),
        (1.6f, "Game is paused"),
    };
    
    public static AgarioPauseMenu CreateAgarioPauseMenu(Vector2u windowSize)
    {
        AgarioPauseMenu result = new();
        
        result.AdoptChild(CreateGamePausedText(windowSize));
        
        return result;
    }

    private static TextNode CreateGamePausedText(Vector2u windowSize)
    {
        TextNode text = TextNode.CreateTextNode();
        text.Position = new(windowSize.X / 2, windowSize.Y / 2);
        
        AddGamePausedAnimationToText(text);

        return text;
    }

    private static void AddGamePausedAnimationToText(TextNode text)
    {
        SequenceNode<string> sequenceNode = SequenceNode<string>.CreateSequenceNode(Elements);
        text.AdoptChild(sequenceNode);
        
        sequenceNode.Sequence.OnFinished = sequenceNode.Sequence.Restart;
        sequenceNode.Sequence.OnElementDue = text.UpdateLine;
        sequenceNode.Sequence.Play();
    }
}