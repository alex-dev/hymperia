#pragma once
#using "PresentationCore.dll"

using namespace System;
using namespace System::Runtime::CompilerServices;
using namespace System::Windows::Media::Media3D;

namespace DirectXOperations
{
  [ExtensionAttribute]
  public ref class MatrixOperationsExtension abstract sealed
  {
  public:
    [ExtensionAttribute]
    static Tuple<Vector3D, Quaternion> ^ __clrcall Decompose(Matrix3D % matrix);

    [ExtensionAttribute]
    static Matrix3D __clrcall ComposeTransform(Vector3D % vector, Quaternion % quaternion);

    [ExtensionAttribute]
    static Matrix3D __clrcall ComposeTransform(Quaternion % quaternion, Vector3D % vector);
  };
}
