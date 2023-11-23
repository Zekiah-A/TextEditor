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
    private DragEdge draggingEdge = DragEdge.None;
    const int dragMargin = 8;

    public MainWindow()
    {
        InitializeComponent();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            SystemDecorations = SystemDecorations.BorderOnly;
            HeaderControls.IsVisible = true;
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
        
        var mousePos = e.GetPosition(this);
        void UpdateRight()
        {
            Width = mousePos.X;
        }
        void UpdateBottom()
        {
            Height = mousePos.Y;
        }
        void UpdateLeft()
        {
            Position = new PixelPoint((int)(Position.X + mousePos.X), Position.Y);
            Width -= mousePos.X;
        }
        void UpdateTop()
        {
            Position = new PixelPoint(Position.X, (int)(Position.Y + mousePos.Y));
            Height -= mousePos.Y;
        }
        
        switch (draggingEdge)
        {
            case DragEdge.Right:
                UpdateRight();
                break;
            case DragEdge.Bottom:
                UpdateBottom();
                break;
            case DragEdge.BottomRight:
                UpdateRight();
                UpdateBottom();
                break;
            case DragEdge.Left:
                UpdateLeft();
                break;
            case DragEdge.Top:
                UpdateTop();
                break;
            case DragEdge.BottomLeft:
                UpdateBottom();
                UpdateLeft();
                break;
            case DragEdge.TopLeft:
                UpdateTop();
                UpdateLeft();
                break;
            case DragEdge.TopRight:
                UpdateTop();
                UpdateRight();
                break;
        }
    }

    private readonly Cursor topLeftCursor = new Cursor(StandardCursorType.TopLeftCorner);
    private readonly Cursor leftCursor = new Cursor(StandardCursorType.LeftSide);
    private readonly Cursor bottomLeftCursor = new Cursor(StandardCursorType.BottomLeftCorner);
    private readonly Cursor bottomCursor = new Cursor(StandardCursorType.BottomSide);
    private readonly Cursor bottomRightCursor = new Cursor(StandardCursorType.BottomRightCorner);
    private readonly Cursor rightCursor = new Cursor(StandardCursorType.RightSide);
    private readonly Cursor topRightCursor = new Cursor(StandardCursorType.TopRightCorner);
    private readonly Cursor topCursor = new Cursor(StandardCursorType.TopSide);

    private DragEdge GetCursorEdge(Point mousePos)
    {
        if (mousePos.X < dragMargin && mousePos.Y < dragMargin)
        {
            return DragEdge.TopLeft;
        }
        if (mousePos.X < dragMargin && mousePos.Y > Height - dragMargin)
        {
            return DragEdge.BottomLeft;
        }
        if (mousePos.X < dragMargin && mousePos.X < dragMargin)
        {
            return DragEdge.Left;
        }
        if (mousePos.X > Width - dragMargin && mousePos.Y < dragMargin)
        {
            return DragEdge.TopRight;
        }
        if (mousePos.X > Width - dragMargin && mousePos.Y > Height - dragMargin)
        {
            return DragEdge.BottomRight;
        }
        if (mousePos.X > Width - dragMargin)
        {
            return DragEdge.Right;
        }
        if (mousePos.Y < dragMargin)
        {
            return DragEdge.Top;
        }
        if (mousePos.Y > Height - dragMargin)
        {
            return DragEdge.Bottom;
        }
        
        return DragEdge.None;
    }

    private Cursor GetDraggingEdgeCursor(DragEdge edge)
    {
        return edge switch
        {
            DragEdge.TopLeft => topLeftCursor,
            DragEdge.Left => leftCursor,
            DragEdge.BottomLeft => bottomLeftCursor,
            DragEdge.Bottom => bottomCursor,
            DragEdge.BottomRight => bottomRightCursor,
            DragEdge.Right => rightCursor,
            DragEdge.TopRight => topRightCursor,
            DragEdge.Top => topCursor,
            DragEdge.None => Cursor.Default,
            _ => Cursor.Default
        };
    }

    private void OnBackgroundPointerPress(object? sender, PointerPressedEventArgs e)
    {
        draggingEdge = GetCursorEdge(e.GetPosition(this));
        if (draggingEdge == DragEdge.None)
        {
            return;
        }

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