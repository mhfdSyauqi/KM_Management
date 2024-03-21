import { useRequest } from '@/api/base/useRequest.js'

export async function GetCategoryReferenceAsync() {
  const result = {
    is_success: true,
    reference: [],
    error: null
  }

  await useRequest
    .get('category/ref')
    .then((res) => {
      result.reference = res.data.data.category_reference
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetCategoryListAsync(uid_reference, layer, is_active) {
  const result = {
    is_success: true,
    category_list: null,
    error: null
  }

    const Payload = {
      Uid_Reference:uid_reference,
      Layer:layer,
      Is_Active:is_active,
    }

  await useRequest
    .post(`Category/GetCategoryList`, Payload)
    .then((res) => {
      result.category_list = res.data.data.category_list
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function PostCategoryListAsync(name, layer, is_active, uid_reference) {
  const Payload = JSON.stringify({
    Name: name,
    Layer: layer,
    Is_Active: is_active,
    Uid_Reference: uid_reference,
  })

  const result = {
    is_success: true,
    error: null
  }
  await useRequest.post('Category/AddNewCategory', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}

export async function PatchCategoryListAsync(
  uid,
  name,
  is_active
) {
  const Payload = JSON.stringify({
    Uid: uid,
    Name: name, 
    is_active: is_active,
  })

  const result = {
    is_success: true,
    error: null
  }

  await useRequest.patch('Category/UpdateCategoryList', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}

