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