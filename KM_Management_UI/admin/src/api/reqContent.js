import { useRequest } from '@/api/base/useRequest.js'

export async function GetGeneralContentsAsync(title_or_category = null, status, page = 1) {
  const articleStatus = status.filter((item) => item !== 'Inactive')
  const isInactive = status.includes('Inactive')

  const Payload = {
    Searched_Title_Or_Category: title_or_category,
    Searched_Article_Status: articleStatus.length === 0 ? null : articleStatus,
    Inactive_Category: !isInactive ? null : isInactive,
    Searched_Page: page
  }

  const { data, statusCode, error } = await useRequest('content').post(Payload).json()

  return {
    is_success: statusCode.value === 200,
    contents: data.value?.data.contents,
    error: error?.value
  }
}

export async function PostContentAsync(title, categoryId, desc, article, addtional) {
  const Payload = {
    Title: title,
    Category_Id: categoryId,
    Description: desc,
    Article: article,
    Additonal_Link: addtional === '' ? null : addtional
  }

  const { statusCode, error } = await useRequest('content/action').post(Payload).json()

  return {
    is_success: statusCode.value === 200,
    error: error?.value
  }
}