### Tle5012bLibrary

Library for work with Encoder (with Tle5012b), only 0x8021 cmd, return angle (double) and CRC check (true/false)

### Usage

```
using FIV.Tle5012b;

...
RotaryEncoder encoder = new RotaryEncoder(int busId=0, int ChipSelect=0);

double res;
if (encoder.Cmd_8021(out res)) Console.WriteLine(res);
```

