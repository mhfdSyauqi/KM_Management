import { useRequest } from '@/api/base/useRequest.js'

export async function GetGeneralContentsAsync(title_or_category = null, status, page = 1) {
  const articleStatus = status.filter((item) => item !== 'Inactive')
  const isInactive = status.includes('Inactive')

  const Payload = JSON.stringify({
    Searched_Title_Or_Category: title_or_category,
    Searched_Article_Status: articleStatus.length === 0 ? null : articleStatus,
    Inactive_Category: !isInactive ? null : isInactive,
    Searched_Page: page
  })

  const result = {
    is_success: true,
    contents: [],
    error: null
  }

  await useRequest
    .post('content', Payload)
    .then((res) => {
      result.contents = res.data.data.contents
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetDetailContentAsync(id) {
  const result = {
    is_success: true,
    content: null,
    error: null
  }

  await useRequest
    .get(`content/${id}`)
    .then((res) => {
      result.content = res.data.data.content
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}
export async function PostContentAsync(
  action,
  title,
  categoryId,
  descHtml,
  desc,
  article,
  additional
) {
  const Payload = JSON.stringify({
    Action: action,
    Title: title,
    Category_Id: categoryId,
    Description_Html: descHtml,
    Description: desc,
    Article: article,
    Additional_Link: additional === '' ? null : additional
  })

  const result = {
    is_success: true,
    error: null
  }

  await useRequest.post('content/action', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}

export async function PatchContentAsync(
  action,
  id,
  newTitle,
  newCategoryId,
  newDescHtml,
  newDesc,
  newArticle,
  newAdditional
) {
  const Payload = JSON.stringify({
    Action: action,
    Id: id,
    Title: newTitle,
    Category_Id: newCategoryId,
    Description_Html: newDescHtml,
    Description: newDesc,
    Article: newArticle,
    Additional_Link: newAdditional === '' ? null : newAdditional
  })

  const result = {
    is_success: true,
    error: null
  }

  await useRequest.patch('content/action', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}