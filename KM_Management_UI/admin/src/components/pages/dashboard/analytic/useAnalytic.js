import { ref } from 'vue'
import {
  GetHitDetailAsync,
  GetHitExcelLink,
  GetHitGeneralAsync,
  GetLeadDetailAsync,
  GetLeadExcelLink,
  GetLeadGeneralAsync,
  GetPopularDetailAsync,
  GetPopularExcelLink,
  GetPopularGeneralAsync
} from '@/api/reqAnalytic.js'

const filterData = ref({
  filterStart: null,
  filterEnd: null,
  filterType: 'Last 30 Days'
})

const fullScreen = ref({
  stats: 'none',
  isDetail: false
})

const dataPopular = ref(null)
const dataHit = ref(null)
const dataLead = ref(null)

async function GetGeneralPopularData() {
  const result = await GetPopularGeneralAsync(
    filterData.value.filterType,
    filterData.value.filterStart,
    filterData.value.filterEnd
  )

  if (!result.is_success || result.popular_general.length === 0) {
    return null
  }

  const backgroundColor = [
    '#9195F6',
    '#E99497',
    '#DBCC95',
    '#638889',
    '#A4907C',
    '#D2AEE9',
    '#C4DFDF',
    '#9BCF53',
    '#8E7AB5',
    '#667059'
  ]

  let data = {
    labels: [],
    datasets: []
  }

  let dataCount = result.popular_general.length

  let labelValue = []

  let firstDataset = {
    backgroundColor: [],
    data: [],
    total: 0
  }
  let secondDataset = {
    backgroundColor: [],
    data: [],
    total: 0
  }
  let thirdDataset = {
    backgroundColor: [],
    data: [],
    total: 0
  }

  for (let index = 0; index <= dataCount - 1; index++) {
    data.labels.push(result.popular_general[index].name)
    labelValue.push(result.popular_general[index].count)

    firstDataset.backgroundColor.push(backgroundColor[index])
    secondDataset.backgroundColor.push(backgroundColor[index])
    thirdDataset.backgroundColor.push(backgroundColor[index])

    firstDataset.total += result.popular_general[index].count
    secondDataset.total += result.popular_general[index].count
    thirdDataset.total += result.popular_general[index].count
  }

  if (dataCount > 8) {
    firstDataset.data = [...labelValue.slice(0, 5), null, null, null, null, null]
    secondDataset.data = [null, null, null, null, null, ...labelValue.slice(5, 8), null, null]
    thirdDataset.data = [
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      ...labelValue.slice(8, dataCount)
    ]

    data.datasets.push(firstDataset, secondDataset, thirdDataset)
  } else if (dataCount > 5 && dataCount <= 8) {
    firstDataset.data = [...labelValue.slice(0, 5), null, null, null]
    secondDataset.data = [null, null, null, null, null, ...labelValue.slice(5, dataCount)]

    data.datasets.push(firstDataset, secondDataset)
  } else if (dataCount <= 5) {
    firstDataset.data = [...labelValue.slice(0, 4)]
    data.datasets.push(firstDataset)
  }

  dataPopular.value = {
    chart: data,
    raw: result.popular_general
  }

  return data
}

async function GetGeneralHitData() {
  const result = await GetHitGeneralAsync(
    filterData.value.filterType,
    filterData.value.filterStart,
    filterData.value.filterEnd
  )

  if (!result.is_success) {
    return null
  }

  dataHit.value = result.hit_general
  return result.hit_general
}

async function GetGeneralLeadData() {
  const result = await GetLeadGeneralAsync(
    filterData.value.filterType,
    filterData.value.filterStart,
    filterData.value.filterEnd
  )

  if (!result.is_success || result.lead_general.length === 0) {
    return null
  }

  dataLead.value = result.lead_general
  return result.lead_general
}

async function GetDetailLeadData() {
  const result = await GetLeadDetailAsync(
    filterData.value.filterType,
    filterData.value.filterStart,
    filterData.value.filterEnd
  )

  if (!result.is_success || result.lead_detail?.length === 0) {
    return null
  }

  return result.lead_detail
}

async function GetDetailHitData(selected) {
  const result = await GetHitDetailAsync(
    filterData.value.filterType,
    filterData.value.filterStart,
    filterData.value.filterEnd,
    selected
  )

  if (!result.is_success) {
    return null
  }

  return result.hit_detail
}

async function GetDetailPopularData(reference) {
  const result = await GetPopularDetailAsync(
    reference,
    filterData.value.filterType,
    filterData.value.filterStart,
    filterData.value.filterEnd
  )

  if (!result.is_success || result.popular_detail?.length === 0) {
    return null
  }

  const backgroundColor = [
    '#9195F6',
    '#E99497',
    '#DBCC95',
    '#638889',
    '#A4907C',
    '#D2AEE9',
    '#C4DFDF',
    '#9BCF53',
    '#8E7AB5',
    '#667059'
  ]

  let data = {
    labels: [],
    datasets: []
  }

  let dataCount = result.popular_detail.length

  let labelValue = []

  let firstDataset = {
    backgroundColor: [],
    data: [],
    total: 0
  }
  let secondDataset = {
    backgroundColor: [],
    data: [],
    total: 0
  }
  let thirdDataset = {
    backgroundColor: [],
    data: [],
    total: 0
  }

  for (let index = 0; index <= dataCount - 1; index++) {
    data.labels.push(result.popular_detail[index].name)
    labelValue.push(result.popular_detail[index].count)

    firstDataset.backgroundColor.push(backgroundColor[index])
    secondDataset.backgroundColor.push(backgroundColor[index])
    thirdDataset.backgroundColor.push(backgroundColor[index])

    firstDataset.total += result.popular_detail[index].count
    secondDataset.total += result.popular_detail[index].count
    thirdDataset.total += result.popular_detail[index].count
  }

  if (dataCount > 8) {
    firstDataset.data = [...labelValue.slice(0, 5), null, null, null, null, null]
    secondDataset.data = [null, null, null, null, null, ...labelValue.slice(5, 8), null, null]
    thirdDataset.data = [
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      ...labelValue.slice(8, dataCount)
    ]

    data.datasets.push(firstDataset, secondDataset, thirdDataset)
  } else if (dataCount > 5 && dataCount <= 8) {
    firstDataset.data = [...labelValue.slice(0, 5), null, null, null]
    secondDataset.data = [null, null, null, null, null, ...labelValue.slice(5, dataCount)]

    data.datasets.push(firstDataset, secondDataset)
  } else if (dataCount <= 5) {
    firstDataset.data = [...labelValue.slice(0, 4)]
    data.datasets.push(firstDataset)
  }

  dataPopular.value.details = result.popular_detail
  return data
}

async function GetExcelPopular() {
  const url = await GetPopularExcelLink(
    filterData.value.filterType,
    filterData.value.filterStart,
    filterData.value.filterEnd
  )

  return window.open(url, '_blank')
}

async function GetExcelHit() {
  const url = await GetHitExcelLink(
    filterData.value.filterType,
    filterData.value.filterStart,
    filterData.value.filterEnd
  )

  return window.open(url, '_blank')
}

async function GetExcelLead() {
  const url = await GetLeadExcelLink(
    filterData.value.filterType,
    filterData.value.filterStart,
    filterData.value.filterEnd
  )

  return window.open(url, '_blank')
}

export {
  filterData,
  fullScreen,
  dataPopular,
  dataHit,
  dataLead,
  GetGeneralPopularData,
  GetGeneralHitData,
  GetGeneralLeadData,
  GetDetailPopularData,
  GetDetailHitData,
  GetDetailLeadData,
  GetExcelPopular,
  GetExcelHit,
  GetExcelLead
}