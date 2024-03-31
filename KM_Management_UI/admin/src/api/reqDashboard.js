import { useRequest } from '@/api/base/useRequest.js'

export async function GetRateAndFeedbackAsync(
  filter_date,
  start_date,
  end_date,
  rating,
  page_limit,
  current_page,
) {
  const Payload = JSON.stringify({
    Filter_Date:filter_date,
    Start_Date:start_date,
    End_Date:end_date,
    Rating:rating,
    Page_limit:page_limit,
    Current_Page:current_page,
  })

  const result = {
    is_success: true,
    rate_and_feedback: [],
    error: null
  }

  await useRequest
    .post('RateAndFeedback/GetRateAndFeedback', Payload)
    .then((res) => {
      result.rate_and_feedback = res.data.data.rate_and_feedback
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}