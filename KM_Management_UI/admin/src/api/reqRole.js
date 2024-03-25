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

export async function GetUsersRoleAsync(
  keyword,
  page = 1,
  limit_page = 10,
  sort_order = null,
  sort_col = null
) {
  const Payload = JSON.stringify({
    Search_Keyword: keyword,
    Searched_Page: page,
    Limit_Page: limit_page,
    Order_Sort: sort_order,
    Column_Sort: sort_col
  })

  const result = {
    is_success: true,
    users_role: {},
    error: null
  }

  await useRequest
    .post('roles', Payload)
    .then((res) => {
      result.users_role = res.data.data.users_role
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response?.data
    })

  return result
}