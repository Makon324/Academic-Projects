# Potion Master - Windows Forms Puzzle Game

## Overview
Potion Master is a Windows Forms puzzle game developed as part of a course in Graphical Programming. The objective is to solve potion-mixing puzzles by transferring colored liquid segments between vials. The game features dynamic difficulty settings, undo mechanics, color themes, and a custom-rendered vial control.

## Features
- **Puzzle Mechanics**:
  - Drag-and-drop pouring between vials.
  - Win condition: All non-empty vials must contain a single uniform color.
  - Dynamic grid layout (up to 7 vials per row).
- **Difficulty Settings**:
  - Three difficulty levels (Easy, Medium, Hard) affecting empty vials and undo limits.
  - Adjustable vial count (4–25) and segment count (2–10).
- **Undo System**:
  - Revert moves with limited undos per game session.
  - Undo counter displayed in the control panel.
- **Color Themes**:
  - Light and Dark themes with consistent styling across all controls and forms.
- **Settings**:
  - Best score saved between sessions.
  - Game settings (difficulty, vial count, theme) persist after restart.


## How to Play
1. **Pouring**:
   - Click and drag from a vial to pour its top color segments into another compatible vial.  
   **Pouring Rules**:
     - Liquid can only be poured into:  
       - An **empty vial**, or  
       - A vial with the **same top color**.  
     - The **entire stack of same-colored segments** is poured at once (partial transfers are not allowed).  
     - Destination vial must have enough space for all transferred segments.
2. **Solving Puzzles**:
   - Arrange all colored segments into vials with uniform colors.
   - A "Congratulations!" message appears upon success.
3. **Controls**:
   - **Undo**: Revert the last move (limited uses based on difficulty).
   - **Next Puzzle**: Start a new puzzle after solving the current one.
   - **Settings**: Adjust difficulty, vial count, and theme via the menu.





