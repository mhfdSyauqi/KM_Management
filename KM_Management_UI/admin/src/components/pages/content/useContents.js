import { ref } from 'vue'
import { GetGeneralContentsAsync } from '@/api/reqContent.js'

const contents = ref([])
const filter = ref({
  status: [],
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

async function GetContentsByFilter() {
  const result = await GetGeneralContentsAsync(
    filter.value.search,
    filter.value.status,
    filter.value.page,
    filter.value.limit,
    filter.value.sort_order,
    filter.value.sort_col
  )

  if (result.is_success) {
    contents.value = result.contents.items

    navigation.value.current = result.contents.curr_page
    navigation.value.previous = result.contents.prev_page
    navigation.value.next = result.contents.next_page
    navigation.value.max = result.contents.max_page
    navigation.value.total = result.contents.total_row

    let startRow =
      result.contents.curr_page === 1
        ? 1
        : result.contents.next_page === null
          ? result.contents.total_row - result.contents.items.length + 1
          : result.contents.items.length + result.contents.prev_page
    let endRow =
      result.contents.next_page === null
        ? result.contents.total_row
        : result.contents.curr_page * result.contents.items.length

    navigation.value.show = `Showing ${startRow}-${endRow} of ${result.contents.total_row} entries`
  }
}

async function HandleSearch(searchItem) {
  filter.value.search = searchItem
  filter.value.page = 1
  return await GetContentsByFilter()
}

async function HandleCheck(isCheck, value) {
  const uniqueSet = new Set(filter.value.status)
  if (isCheck) uniqueSet.add(value)
  if (!isCheck) uniqueSet.delete(value)
  filter.value.status = [...uniqueSet]
  filter.value.page = 1
  return await GetContentsByFilter()
}

async function HandlePagination(nextPage) {
  filter.value.page = nextPage
  return await GetContentsByFilter()
}

async function HandlingSort(column) {
  if (filter.value.sort_order === 'ASC') {
    filter.value.sort_order = 'DESC'
  } else {
    filter.value.sort_order = 'ASC'
  }
  filter.value.sort_col = column
  return await GetContentsByFilter()
}

async function HandlingPageLimit(limit) {
  filter.value.limit = limit
  return await GetContentsByFilter()
}

export {
  contents,
  navigation,
  GetContentsByFilter,
  HandleSearch,
  HandleCheck,
  HandlePagination,
  HandlingSort,
  HandlingPageLimit
}