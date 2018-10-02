#pragma once
#include <tuple>
#include "DirectXMath.h"

template<class ...Types> using tuple = std::tuple<Types...>;
using namespace DirectX;

namespace DirectXOperations
{
  class Unmanaged abstract final
  {
  public:
    static tuple<XMFLOAT3, XMFLOAT4> __stdcall Decompose(XMFLOAT4X4 const & matrix);
    static XMFLOAT4X4 __stdcall Compose(XMFLOAT3 const & translation, XMFLOAT4 const & rotation);

  private:
    static XMMATRIX __fastcall LoadXMFloat4x4(XMFLOAT4X4 const & matrix);
    static XMVECTOR __fastcall LoadXMFloat4(XMFLOAT4 const & quaternion);
    static XMVECTOR __fastcall LoadXMFloat3(XMFLOAT3 const & vector);

    static XMFLOAT4X4 __fastcall StoreXMFloat4x4(XMMATRIX const & matrix);
    static XMFLOAT4 __fastcall StoreXMFloat4(XMVECTOR const & quaternion);
    static XMFLOAT3 __fastcall StoreXMFloat3(XMVECTOR const & vector);
  };
}