using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.TurnManager;

namespace RPG.Interfaces 
{
    public interface ISelectable
    {
        void OnSelect(PlayerHolder player);
    }

    public interface IDeselect
    {
        void OnDeselect(PlayerHolder player);
    }

    public interface IHighlight
    {
        void OnHighlight(PlayerHolder player);
    }

    public interface IDeHighlight
    {
        void OnDeHighlight(PlayerHolder player);
    }

    public interface IDetectable
    {
        RPG.Grid.Node OnDetect();
    }
}