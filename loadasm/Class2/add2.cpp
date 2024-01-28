#include <iostream>

extern "C"
__declspec(dllexport)
int add2(int a, int b)
{
#ifdef __CLR_VER 
  System::Console::WriteLine(L"hello from clr!");
#endif
  int result = a + b;
  std::cout << a << "+" << b << "=" << result << std::endl;
  return result;
}
