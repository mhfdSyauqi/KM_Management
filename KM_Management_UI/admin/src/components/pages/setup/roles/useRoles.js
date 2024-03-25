import { ref } from 'vue'
import { GetUsersRoleAsync } from '@/api/reqRole.js'

const usersRole = ref([])

const filter = ref({
  search: null,
  page: 1,
  limit: 10,
  sort_order: null,
  sort_col: null
})

const navigation = ref({
  current: 1,
  previous: null,
  next: null,
  max: null,
  show: 'Showing 0 of 0 Rows',
  total: 0
})

async function GetUsersRoleByFilter() {
  const result = await GetUsersRoleAsync(
    filter.value.search,
    filter.value.page,
    filter.value.limit,
    filter.value.sort_order,
    filter.value.sort_col
  )

  if (result.is_success) {
    usersRole.value = result.users_role.items

    navigation.value.current = result.users_role.curr_page
    navigation.value.previous = result.users_role.prev_page
    navigation.value.next = result.users_role.next_page
    navigation.value.max = result.users_role.max_page
    navigation.value.total = result.users_role.total_row

    let startRow =
      result.users_role.curr_page === 1
        ? 1
        : result.users_role.next_page === null
          ? result.users_role.total_row - result.users_role.items.length + 1
          : result.users_role.items.length + result.users_role.prev_page
    let endRow =
      result.users_role.next_page === null
        ? result.users_role.total_row
        : result.users_role.curr_page * result.users_role.items.length

    navigation.value.show = `Showing ${startRow}-${endRow} of ${result.users_role.total_row} entries`
  }
}

async function HandleSearch(searchItem) {
  filter.value.search = searchItem
  filter.value.page = 1
  return await GetUsersRoleByFilter()
}

async function HandlePagination(nextPage) {
  filter.value.page = nextPage
  return await GetUsersRoleByFilter()
}

async function HandlingSort(column) {
  if (filter.value.sort_order === 'ASC') {
    filter.value.sort_order = 'DESC'
  } else {
    filter.value.sort_order = 'ASC'
  }
  filter.value.sort_col = column
  return await GetUsersRoleByFilter()
}

async function HandlingPageLimit(limit) {
  console.log(limit)
  filter.value.limit = limit
  return await GetUsersRoleByFilter()
}

export {
  usersRole,
  navigation,
  GetUsersRoleByFilter,
  HandleSearch,
  HandlePagination,
  HandlingSort,
  HandlingPageLimit
}