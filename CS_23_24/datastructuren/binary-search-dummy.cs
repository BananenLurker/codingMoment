// x is de vereiste grootte
list<int> a = new list<int>;

// Terminate if:
// a[j] >= x;
// a[j - 1] < x;

int i = -1; j = int n; // Waarbij n een getal is dat in ieder geval groter is dan x
while (!(i = j - 1))
{
  int m = (i + j) / 2 // Behoeft geen Math.Floor door integer afronding dor c#
    if (a[m] < x)
  {
    i = m;
  }
  else
  {
    j = m;
  }
}
return j;