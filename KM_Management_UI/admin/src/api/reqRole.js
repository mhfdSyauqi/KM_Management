import { useRequest } from '@/api/base/useRequest.js'

export async function GetUserRole() {
  const result = {
    is_success: true,
    user_role: {},
    error: null
  }

  await useRequest
    .get('roles')
    .then((res) => {
      result.user_role = res.data.data.user_information
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response?.data
    })

  return result
}