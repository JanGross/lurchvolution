using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Movement : MonoBehaviour {

    [System.Serializable]
    class MovementDirection
    {
        [SerializeField]
        private bool enabled = false;
        [SerializeField]
        private enum Direction { Left_Right, Up_Down, Fwd_Bkw }
        [SerializeField]
        private Direction direction;
        [SerializeField]
        private bool loop = false;
        [SerializeField]
        private bool goBack = false;
        [SerializeField]
        private float distance = 100;
        [SerializeField]
        private float speed = 0;
        [SerializeField]
        private float dist;

        private Vector3 initialPosition;
        public void SetInitialPosition(Vector3 initPos)
        {
            initialPosition = initPos;
        }

        public Vector3 Move(Vector3 dirIn)
        {
            int dir = (int)direction;
            Vector3 dirOut = dirIn;
            enabled = loop ? true : enabled;

            if (enabled)
            {
                dist = initialPosition[dir] - dirOut[dir];

                if (goBack)
                {
                    dirOut[dir] += speed * Time.deltaTime;
                }
                else
                {
                    dirOut[dir] -= speed * Time.deltaTime;
                }

                if (dist > distance && !goBack)
                {
                    if (loop)
                    {
                        goBack = true;
                    }
                    else
                    {
                        enabled = false;
                    }
                }
                else if (dist <= 0)
                {
                    goBack = false;

                }
            }
            return dirOut;
        }
    }

    [SerializeField]
    private List<MovementDirection> directions;
	// Use this for initialization
	void Start () {
        foreach (MovementDirection direction in directions)
        {
            direction.SetInitialPosition(transform.position);
        }
    }
	
	// Update is called once per frame
	void Update () {
		foreach ( MovementDirection direction in directions)
        {
            transform.position = direction.Move(transform.position);
        }
	}
}
