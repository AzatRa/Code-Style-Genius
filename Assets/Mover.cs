using UnityEngine;

public class Mover : MonoBehaviour
{
    private float _speed;
    public Transform AllPlacespoint;
    Transform[] arrayPlaces;
    private int _numberOfPlaceInArrayPlaces;

    private void Start()
    {
        arrayPlaces = new Transform[AllPlacespoint.childCount];

        for (int i = 0; i < AllPlacespoint.childCount; i++)
            arrayPlaces[i] = AllPlacespoint.GetChild(i).GetComponent<Transform>();
    }

    private void Update()
    {
        var _pointByNumberInArray = arrayPlaces[_numberOfPlaceInArrayPlaces];
        transform.position = Vector3.MoveTowards(transform.position, _pointByNumberInArray.position, _speed * Time.deltaTime);

        if (transform.position == _pointByNumberInArray.position) NextPlaceTakerLogic();
    }

    private Vector3 NextPlaceTakerLogic()
    {
        _numberOfPlaceInArrayPlaces++;

        if (_numberOfPlaceInArrayPlaces == arrayPlaces.Length)
            _numberOfPlaceInArrayPlaces = 0;

        var thisPointVector = arrayPlaces[_numberOfPlaceInArrayPlaces].transform.position;
        transform.forward = thisPointVector - transform.position;
        return thisPointVector;
    }
}