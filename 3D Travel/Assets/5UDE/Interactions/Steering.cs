/*
Copyright ©2017. The University of Texas at Dallas. All Rights Reserved. 

Permission to use, copy, modify, and distribute this software and its documentation for 
educational, research, and not-for-profit purposes, without fee and without a signed 
licensing agreement, is hereby granted, provided that the above copyright notice, this 
paragraph and the following two paragraphs appear in all copies, modifications, and 
distributions. 

Contact The Office of Technology Commercialization, The University of Texas at Dallas, 
800 W. Campbell Road (AD15), Richardson, Texas 75080-3021, (972) 883-4558, 
otc@utdallas.edu, https://research.utdallas.edu/otc for commercial licensing opportunities.

IN NO EVENT SHALL THE UNIVERSITY OF TEXAS AT DALLAS BE LIABLE TO ANY PARTY FOR DIRECT, 
INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES, INCLUDING LOST PROFITS, ARISING 
OUT OF THE USE OF THIS SOFTWARE AND ITS DOCUMENTATION, EVEN IF THE UNIVERSITY OF TEXAS AT 
DALLAS HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

THE UNIVERSITY OF TEXAS AT DALLAS SPECIFICALLY DISCLAIMS ANY WARRANTIES, INCLUDING, BUT 
NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE. THE SOFTWARE AND ACCOMPANYING DOCUMENTATION, IF ANY, PROVIDED HEREUNDER IS 
PROVIDED "AS IS". THE UNIVERSITY OF TEXAS AT DALLAS HAS NO OBLIGATION TO PROVIDE 
MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
*/

using UnityEngine;
using System.Collections;

public class Steering : MonoBehaviour {

	// Enumerate the states of steering
	public enum SteeringState {
		NotSteering,
		SteeringForward,
		SteeringBackward,
        TurningRight,
        TurningLeft
	};
	
    // Inspector parameters
    [Tooltip("The tracking device used to determine absolute direction for steering.")]
    public CommonTracker tracker;

    [Tooltip("The controller joystick used to determine relative direction (forward/backward) and speed.")]
	public CommonAxis joystick;

	[Tooltip("A button required to be pressed to activate steering.")]
	public CommonButton button;  

    [Tooltip("The space that is translated by this interaction. Usually set to the physical tracking space.")]
    public CommonSpace space;

    [Tooltip("The median speed for movement expressed in meters per second.")]
    public float speed = 1.0f;

	// Private interaction variables
	private SteeringState state;

	// Called at the end of the program initialization
	void Start () {

		// Set initial steering state to not steering
		state = SteeringState.NotSteering;
	}
		
    // FixedUpdate is not called every graphical frame but rather every physics frame
	void FixedUpdate () {
		// If the button is not pressed
		if (!button.GetPress())
		{

			// Change state to not steering 
			state = SteeringState.NotSteering;
		}

		// check turnning of forwaring
		else if (Mathf.Abs(joystick.GetAxis().y) > Mathf.Abs(joystick.GetAxis().x))
		{
			// If the joystick is pressed forward
            if (joystick.GetAxis().y > 0.0f)
			{

				// Change state to steering forward
				state = SteeringState.SteeringForward;
            } else {

				// Change state to steering backward
				state = SteeringState.SteeringBackward;
            }
        }

        // turn left or right
        else if (joystick.GetAxis().x > 0.0f) {
            // Turn right
            state = SteeringState.TurningRight;
        } else {
            // Turn left
            state = SteeringState.TurningLeft;
        }


        // If state is not steering
        if (state == SteeringState.NotSteering)
        {
            //Do nothing
        }

        // If state is steering forward
        else if (state == SteeringState.SteeringForward)
        {
            Vector3 direction = tracker.transform.forward;
            direction.y = 0.0f;
            // Translate the space based on the tracker's absolute forward direction and the joystick's forward value
            space.transform.position += joystick.GetAxis().y * direction * speed * Time.deltaTime;
        }

        // If state is steering backward
        else if (state == SteeringState.SteeringBackward)
        {

            Vector3 direction = tracker.transform.forward;
            direction.y = 0.0f;
            // Translate the space based on the tracker's absolute forward direction and the joystick's backward value
            space.transform.position += joystick.GetAxis().y * direction * speed * Time.deltaTime;
        }
		// If state is steering backward
        else if (state == SteeringState.TurningLeft)
		{
			//rotate y aixs 
			space.transform.Rotate(0,joystick.GetAxis().x * 10 * speed * Time.deltaTime,0);
		}
		// If state is steering backward
        else if (state == SteeringState.TurningRight)
		{

			//rotate y aixs 
			space.transform.Rotate(0, joystick.GetAxis().x *10* speed * Time.deltaTime, 0);
		}
	}
}