import { BASE_URL, useRequest } from '@/api/base/useRequest.js'

export async function GetPopularGeneralAsync(filterType, filterStart, filterEnd) {
  const result = {
    is_success: true,
    popular_general: [],
    error: null
  }

  const Payload = JSON.stringify({
    Filter: filterType,
    Start_Date: filterStart,
    End_Date: filterEnd
  })

  await useRequest
    .post('analytic/popular/general', Payload)
    .then((res) => {
      result.popular_general = res.data.data.popular_general
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetHitGeneralAsync(filterType, filterStart, filterEnd) {
  const result = {
    is_success: true,
    hit_general: {},
    error: null
  }

  const Payload = JSON.stringify({
    Filter: filterType,
    Start_Date: filterStart,
    End_Date: filterEnd
  })

  await useRequest
    .post('analytic/hit/general', Payload)
    .then((res) => {
      result.hit_general = res.data.data.hit_general
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetLeadGeneralAsync(filterType, filterStart, filterEnd) {
  const result = {
    is_success: true,
    lead_general: {},
    error: null
  }

  const Payload = JSON.stringify({
    Filter: filterType,
    Start_Date: filterStart,
    End_Date: filterEnd
  })

  await useRequest
    .post('analytic/lead/general', Payload)
    .then((res) => {
      result.lead_general = res.data.data.lead_general
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetLeadDetailAsync(filterType, filterStart, filterEnd) {
  const result = {
    is_success: true,
    lead_detail: {},
    error: null
  }

  const Payload = JSON.stringify({
    Filter: filterType,
    Start_Date: filterStart,
    End_Date: filterEnd
  })

  await useRequest
    .post('analytic/lead/detail', Payload)
    .then((res) => {
      result.lead_detail = res.data.data.lead_detail
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetHitDetailAsync(filter, filterStart, filterEnd, filterType) {
  const result = {
    is_success: true,
    hit_detail: {},
    error: null
  }

  const Payload = JSON.stringify({
    Filter: filter,
    Start_Date: filterStart,
    End_Date: filterEnd,
    Type: filterType
  })

  await useRequest
    .post('analytic/hit/detail', Payload)
    .then((res) => {
      result.hit_detail = res.data.data.hit_detail
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetPopularDetailAsync(reference, filterType, filterStart, filterEnd) {
  const result = {
    is_success: true,
    popular_detail: {},
    error: null
  }

  const Payload = JSON.stringify({
    Reference: reference,
    Filter: filterType,
    Start_Date: filterStart,
    End_Date: filterEnd
  })

  await useRequest
    .post('analytic/popular/detail', Payload)
    .then((res) => {
      result.popular_detail = res.data.data.popular_detail
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetPopularExcelLink(filterType, filterStart, filterEnd) {
  let downloadLink = BASE_URL + '/analytic/popular/excel?'

  if (filterType === 'Custom') {
    downloadLink += `Start_Date=${filterStart}&End_Date=${filterEnd}&Filter=${filterType}`
  } else {
    downloadLink += `Filter=${filterType}`
  }

  return downloadLink
}

export async function GetHitExcelLink(filterType, filterStart, filterEnd) {
  let downloadLink = BASE_URL + '/analytic/hit/excel?'

  if (filterType === 'Custom') {
    downloadLink += `Start_Date=${filterStart}&End_Date=${filterEnd}&Filter=${filterType}`
  } else {
    downloadLink += `Filter=${filterType}`
  }

  return downloadLink
}

export async function GetLeadExcelLink(filterType, filterStart, filterEnd) {
  let downloadLink = BASE_URL + '/analytic/lead/excel?'

  if (filterType === 'Custom') {
    downloadLink += `Start_Date=${filterStart}&End_Date=${filterEnd}&Filter=${filterType}`
  } else {
    downloadLink += `Filter=${filterType}`
  }

  return downloadLink
}