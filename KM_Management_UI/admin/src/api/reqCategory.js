import { useRequest } from '@/api/base/useRequest.js'

export async function GetCategoryReferenceAsync() {
  const { data, statusCode, error } = await useRequest('category/ref').get().json()

  return {
    is_success: statusCode.value === 200,
    reference: data.value?.data.category_reference,
    error: error?.value
  }
}