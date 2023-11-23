using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace TextEditorAvalonia.Views;

public partial class MainWindow : Window
{
    private bool dragging;
    private WindowEdge? draggingEdge = null;
    private readonly Cursor topLeftCursor = new Cursor(StandardCursorType.TopLeftCorner);
    private readonly Cursor leftCursor = new Cursor(StandardCursorType.LeftSide);
    private readonly Cursor bottomLeftCursor = new Cursor(StandardCursorType.BottomLeftCorner);
    private readonly Cursor bottomCursor = new Cursor(StandardCursorType.BottomSide);
    private readonly Cursor bottomRightCursor = new Cursor(StandardCursorType.BottomRightCorner);
    private readonly Cursor rightCursor = new Cursor(StandardCursorType.RightSide);
    private readonly Cursor topRightCursor = new Cursor(StandardCursorType.TopRightCorner);
    private readonly Cursor topCursor = new Cursor(StandardCursorType.TopSide);
    const int dragMargin = 8;

    public MainWindow()
    {
        InitializeComponent();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            SystemDecorations = SystemDecorations.BorderOnly;
            HeaderControls.IsVisible = true;
        }
        else
        {
            ExtendClientAreaToDecorationsHint = true;
        }
    }

    private void OnMinimiseClicked(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void OnMaximiseRestoreClicked(object? sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            MaximiseRestore.Content = "\ue922";
        }
        else
        {
            WindowState = WindowState.Maximized;
            MaximiseRestore.Content = "\ue923";
        }
    }

    private void OnCloseClicked(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IControlledApplicationLifetime controlledLifetime)
        {
            controlledLifetime.Shutdown();
        } 
    }

    private void OnBackgroundPointerMove(object? sender, PointerEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            return;
        }
        if (!dragging)
        {
            var hoveringEdge = GetCursorEdge(e.GetPosition(this));
            Cursor = GetDraggingEdgeCursor(hoveringEdge);
            return;
        }
    }

    private WindowEdge? GetCursorEdge(Point mousePos)
    {
        if (mousePos.X < dragMargin && mousePos.Y < dragMargin)
        {
            return WindowEdge.NorthWest;
        }
        if (mousePos.X < dragMargin && mousePos.Y > Height - dragMargin)
        {
            return WindowEdge.SouthWest;
        }
        if (mousePos.X < dragMargin && mousePos.X < dragMargin)
        {
            return WindowEdge.West;
        }
        if (mousePos.X > Width - dragMargin && mousePos.Y < dragMargin)
        {
            return WindowEdge.NorthEast;
        }
        if (mousePos.X > Width - dragMargin && mousePos.Y > Height - dragMargin)
        {
            return WindowEdge.SouthEast;
        }
        if (mousePos.X > Width - dragMargin)
        {
            return WindowEdge.East;
        }
        if (mousePos.Y < dragMargin)
        {
            return WindowEdge.North;
        }
        if (mousePos.Y > Height - dragMargin)
        {
            return WindowEdge.South;
        }
        
        return null;
    }

    private Cursor GetDraggingEdgeCursor(WindowEdge? edge)
    {
        return edge switch
        {
            WindowEdge.NorthWest => topLeftCursor,
            WindowEdge.West => leftCursor,
            WindowEdge.SouthWest => bottomLeftCursor,
            WindowEdge.South => bottomCursor,
            WindowEdge.SouthEast => bottomRightCursor,
            WindowEdge.East => rightCursor,
            WindowEdge.NorthEast => topRightCursor,
            WindowEdge.North => topCursor,
            _ => Cursor.Default
        };
    }

    private void OnBackgroundPointerPress(object? sender, PointerPressedEventArgs e)
    {
        draggingEdge = GetCursorEdge(e.GetPosition(this));
        if (draggingEdge == null)
        {
            BeginMoveDrag(e);
            
            return;
        }

        BeginResizeDrag(draggingEdge.Value, e);
        Cursor = GetDraggingEdgeCursor(draggingEdge);
        dragging = true;
    }

    private void OnBackgroundPointerRelease(object? sender, PointerReleasedEventArgs e)
    {
        if (dragging)
        {   
            Cursor = Cursor.Default;
        }
        
        dragging = false;
    }
}