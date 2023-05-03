using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use in combination with AnimationEvents.
/// Usefull for things like a ComboSystem so that the Player doesnt have to spam the Attack button for the next combo
/// </summary>
public class InputBuffer : MonoBehaviour
{
    /// <summary>
    /// The max amount of InputEvent in the Buffer
    /// </summary>
    public int MaxBufferSize = 3;

    /// <summary>
    /// How long a InputEvent is valid
    /// </summary>
    public float InputBufferTime = 1.0f;
    
    
    private Queue<InputEvent> inputBuffer = new Queue<InputEvent>();

    /// <summary>
    /// Variable to check if the next InputEvent in the Queue can be processed
    /// </summary>
    private bool freeBuffer = true;

    public void Update()
    {
        // Get all the Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddInputEventToBuffer(InputAction.Jump);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AddInputEventToBuffer(InputAction.Attack);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddInputEventToBuffer(InputAction.Dodge);
        }
    }

    private void FixedUpdate()
    {
        // Check if their are any InputEvents in the Queue and if we are ready to process a InputEvent
        if (inputBuffer.Count > 0 && freeBuffer == true)
        {
            InputEvent currentInputEvent = inputBuffer.Peek();

            // If InputEvent is older then the InputBufferTime
            // then remove it from the Buffer without invoking any action
            if ( (Time.time - currentInputEvent.Time) > InputBufferTime )
            {
                inputBuffer.Dequeue();
                return;
            }

            // If the InputEvent is valid block the processing of the next Input till the Action is finished
            freeBuffer = false;
            inputBuffer.Dequeue();
            ProcessInputEvent(currentInputEvent);
        }
    }

    /// <summary>
    /// Invoke all the Actions for a specific InputAction
    /// </summary>
    /// <param name="inputEvent"> The InputEvent to process </param>
    public void ProcessInputEvent(InputEvent inputEvent)
    {
        switch (inputEvent.InputAction)
        {
            case InputAction.Dodge:
                break;
            case InputAction.Jump:
                break;
            case InputAction.Attack:
                break;
        }
    }

    /// <summary>
    /// Clears all the InputEvents
    /// </summary>
    public void ClearInputBuffer()
    {
        inputBuffer.Clear();
    }

    /// <summary>
    /// Lets the next InputEvent be processed
    /// </summary>
    public void InputActionFinished()
    {
        freeBuffer = true;
    }

    /// <summary>
    /// Adds a new InputEvent to the Buffer
    /// </summary>
    /// <param name="inputAction"></param>
    /// <returns> True if the InputEvent could be added to the Buffer </returns>
    public bool AddInputEventToBuffer(InputAction inputAction)
    {
        if (inputBuffer.Count < MaxBufferSize)
        {
            InputEvent inputEvent = new InputEvent(inputAction, Time.time);
            inputBuffer.Enqueue(inputEvent);
            return true;
        }
        return false;
    }
}

public class InputEvent
{
    public InputAction InputAction;
    public float Time;

    public InputEvent(InputAction inputAction, float time)
    {
        this.InputAction = inputAction;
        Time = time;
    }
}

// All the InputActions that can be stored in the buffer
public enum InputAction
{
    Attack,
    Jump,
    Dodge
}