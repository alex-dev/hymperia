#include "Unmanaged.h"

using namespace DirectXOperations;

tuple<XMFLOAT3, XMFLOAT4> __stdcall Unmanaged::Decompose(XMFLOAT4X4 const & matrix)
{
  XMVECTOR translation, rotation, scale;
  XMMatrixDecompose(&scale, &rotation, &translation, LoadXMFloat4x4(matrix));
  return tuple<XMFLOAT3, XMFLOAT4>(StoreXMFloat3(translation), StoreXMFloat4(rotation));
}

XMFLOAT4X4 __stdcall Unmanaged::Compose(XMFLOAT3 const & translation, XMFLOAT4 const & rotation)
{
  auto const _rotation = XMMatrixRotationQuaternion(LoadXMFloat4(rotation));
  auto const _translation = XMMatrixTranslationFromVector(LoadXMFloat3(translation));
  return StoreXMFloat4x4(_rotation * _translation);
}

XMMATRIX __fastcall Unmanaged::LoadXMFloat4x4(XMFLOAT4X4 const & matrix)
{
  return XMLoadFloat4x4(&matrix);
}

XMVECTOR __fastcall Unmanaged::LoadXMFloat4(XMFLOAT4 const & quaternion)
{
  return XMLoadFloat4(&quaternion);
}

XMVECTOR __fastcall Unmanaged::LoadXMFloat3(XMFLOAT3 const & vector)
{
  return XMLoadFloat3(&vector);
}

XMFLOAT4X4 __fastcall Unmanaged::StoreXMFloat4x4(XMMATRIX const & matrix)
{
  XMFLOAT4X4 value;
  XMStoreFloat4x4(&value, matrix);
  return value;
}

XMFLOAT4 __fastcall Unmanaged::StoreXMFloat4(XMVECTOR const & quaternion)
{
  XMFLOAT4 value;
  XMStoreFloat4(&value, quaternion);
  return value;
}

XMFLOAT3 __fastcall Unmanaged::StoreXMFloat3(XMVECTOR const & vector)
{
  XMFLOAT3 value;
  XMStoreFloat3(&value, vector);
  return value;
}

