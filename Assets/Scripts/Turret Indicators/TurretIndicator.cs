using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public abstract class TurretIndicator : MonoBehaviour
{
    [SerializeField] private Color color = Color.yellow;
    [SerializeField] private float width = 0.05f;
    [SerializeField] private int sortingOrder = -1;
    protected float ToLocal(float worldLength) => worldLength / transform.lossyScale.x;

    protected LineRenderer lr;
    protected Turret turret;

    protected virtual void Start()
    {
        lr = GetComponent<LineRenderer>();
        turret = GetComponentInParent<Turret>();
        if (turret == null)
        {
            Debug.LogError($"{name}: no Turret found on a parent object.");
            enabled = false;
            return;
        }

        lr.useWorldSpace = false;   
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = width;
        lr.sortingOrder = sortingOrder;

        Draw();
    }

    public void Redraw() => Draw();

    protected abstract void Draw();
}