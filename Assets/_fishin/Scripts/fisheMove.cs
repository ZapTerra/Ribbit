using UnityEngine;

public class fisheMove : MonoBehaviour
{
	public Animator animator;
	public Collider2D sightArea;
	public float rotationSpeed = 2f;
	public float speed = 1f;
	public float frequencyOfMove = 5f;

	private lilieArray e;
	private Vector3 startPosition;
	private Vector3 goToPosition;
	private bool isMove = false;

	// Start is called before the first frame update
	void Start()
	{
		startPosition = transform.position;
		e = transform.parent.gameObject.GetComponent<liliePade>().e;
		RandomizeGoToPosition();
		transform.up = goToPosition - transform.position;
		transform.position = goToPosition;
		sightArea.transform.localScale *= e.liliePadeSpacing * ((e.width + e.height) / 2);
		InvokeRepeating("RandomizeGoToPosition", Random.Range(1f, frequencyOfMove), frequencyOfMove);
	}

	// Update is called once per frame
	void Update()
	{
		animator.SetBool("isMove", isMove);
		if (!Camera.main.GetComponent<RecognizerScript>().enabled)
		{
			if (Vector2.Distance(transform.position, goToPosition) < .1f)
			{
				isMove = false;
			}

			if (isMove)
			{
				transform.up = Vector3.Lerp(transform.up, (goToPosition - transform.position), Time.deltaTime * rotationSpeed);
				transform.position += transform.up * Time.deltaTime * speed;
			}
		}
	}

    private void LateUpdate()
    {
		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
	}

    void RandomizeGoToPosition()
	{
		isMove = true;
		goToPosition = Random.insideUnitCircle * e.liliePadeSpacing / 2;
		goToPosition = new Vector3(goToPosition.x * e.width, goToPosition.y * e.height);
		goToPosition += startPosition;
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "bobe" && !Camera.main.GetComponent<RecognizerScript>().enabled)
        {
			goToPosition = collision.gameObject.transform.position;
			isMove = true;
		}			
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "bobe" && !Camera.main.GetComponent<RecognizerScript>().enabled)
		{
			Invoke("isMove = false;", 1f);
		}
	}
}
