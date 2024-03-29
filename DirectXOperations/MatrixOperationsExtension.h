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
	/// <summary>Decompose a <paramref name="matrix" /> in <see cref="Vector3D" /> and <see cref="Quaternion" />.</summary>
    [ExtensionAttribute]
    static Tuple<Vector3D, Quaternion> ^ __clrcall Decompose(Matrix3D % matrix);

	/// <summary>Compose a <see cref="Matrix3D" /> from <paramref name="vector" /> and <paramref name="quaternion" />.</summary>
    [ExtensionAttribute]
    static Matrix3D __clrcall ComposeTransform(Vector3D % vector, Quaternion % quaternion);

	/// <summary>Compose a <see cref="Matrix3D" /> from <paramref name="vector" /> and <paramref name="quaternion" />.</summary>
	[ExtensionAttribute]
    static Matrix3D __clrcall ComposeTransform(Quaternion % quaternion, Vector3D % vector);
  };
}
