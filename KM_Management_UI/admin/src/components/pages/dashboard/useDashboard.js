import { ref } from 'vue'
import {  GetRateAndFeedbackAsync} from '@/api/reqDashboard.js'

const rate_and_feedback = ref([])
const summary = ref("")
const filter = ref({
  filter_date:"",
  start_date:"",
  end_date:"",
  rating:"",
  page_limit:null,
  current_page:null,
})

const navigation = ref({
  current: 1,
  previous: null,
  next: null,
  max: null,
  show: 'Showing 0 of 0 Rows',
  total: 0
})

async function GetRateAndFeedbackByFilter() {
  const result = await GetRateAndFeedbackAsync(
    filter.value.filter_date,
    filter.value.start_date,
    filter.value.end_date,
    filter.value.rating,
    filter.value.page_limit,
    filter.value.current_page
  )

  if (result.is_success) {
    rate_and_feedback.value = result.rate_and_feedback.items
    summary.value = result.rate_and_feedback.summary
    navigation.value.current = result.rate_and_feedback.curr_page
    navigation.value.previous = result.rate_and_feedback.prev_page
    navigation.value.next = result.rate_and_feedback.next_page
    navigation.value.max = result.rate_and_feedback.max_page
    navigation.value.total = result.rate_and_feedback.total_row

    let startRow =
      result.rate_and_feedback.curr_page === 1
        ? 1
        : result.rate_and_feedback.next_page === null
          ? result.rate_and_feedback.total_row - result.rate_and_feedback.items.length + 1
          : result.rate_and_feedback.items.length + result.rate_and_feedback.prev_page
    let endRow =
      result.rate_and_feedback.next_page === null
        ? result.rate_and_feedback.total_row
        : result.rate_and_feedback.curr_page * result.rate_and_feedback.items.length

    navigation.value.show = `Showing ${startRow}-${endRow} of ${result.rate_and_feedback.total_row} entries`
  }
}



async function HandleCheck(isCheck, value) {
  const uniqueSet = new Set(filter.value.status)
  if (isCheck) uniqueSet.add(value)
  if (!isCheck) uniqueSet.delete(value)
  filter.value.status = [...uniqueSet]
  filter.value.page = 1
  return await GetRateAndFeedbackByFilter()
}

async function HandlePagination(nextPage) {
  filter.value.page = nextPage
  return await GetRateAndFeedbackByFilter()
}

async function HandlingPageLimit(limit) {
  filter.value.limit = limit
  return await GetRateAndFeedbackByFilter()
}

export {
  filter,
  summary,
  rate_and_feedback,
  navigation,
  GetRateAndFeedbackByFilter,
  HandleCheck,
  HandlePagination,
  HandlingPageLimit
}