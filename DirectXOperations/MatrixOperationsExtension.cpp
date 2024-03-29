#pragma warning(disable: 4244 4561)
#include "Unmanaged.h"
#include "MatrixOperationsExtension.h"

using std::tie;
using namespace DirectXOperations;

Tuple<Vector3D, Quaternion> ^ __clrcall MatrixOperationsExtension::Decompose(Matrix3D % matrix)
{
  XMFLOAT3 vector;
  XMFLOAT4 quaternion;
  XMFLOAT4X4 const _matrix(
    matrix.M11, matrix.M12, matrix.M13, matrix.M14,
    matrix.M21, matrix.M22, matrix.M23, matrix.M24,
    matrix.M31, matrix.M32, matrix.M33, matrix.M34,
    matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ, matrix.M44);

  std::tie(vector, quaternion) = Unmanaged::Decompose(_matrix);

  return gcnew Tuple<Vector3D, Quaternion>(
    Vector3D(vector.x, vector.y, vector.z),
    Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w));
}

Matrix3D __clrcall MatrixOperationsExtension::ComposeTransform(Vector3D % vector, Quaternion % quaternion)
{
  return ComposeTransform(quaternion, vector);
}

Matrix3D __clrcall MatrixOperationsExtension::ComposeTransform(Quaternion % quaternion, Vector3D % vector)
{
  XMFLOAT4 const _quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
  XMFLOAT3 const _vector(vector.X, vector.Y, vector.Z);

  auto const arr = Unmanaged::Compose(_vector, _quaternion);

  return Matrix3D(
    arr(0, 0), arr(0, 1), arr(0, 2), arr(0, 3),
    arr(1, 0), arr(1, 1), arr(1, 2), arr(1, 3),
    arr(2, 0), arr(2, 1), arr(2, 2), arr(2, 3),
    arr(3, 0), arr(3, 1), arr(3, 2), arr(3, 3));
}
