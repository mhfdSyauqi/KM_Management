import { useRequest } from '@/api/base/useRequest.js'

export async function GetCategoryReferenceAsync() {
  const result = {
    is_success: true,
    reference: [],
    error: null
  }

  await useRequest
    .get('category/ref')
    .then((res) => {
      result.reference = res.data.data.category_reference
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetCategoryListAsync(uid_reference, layer, is_active) {
  const result = {
    is_success: true,
    category_list: null,
    error: null
  }

    const Payload = {
      Uid_Reference:uid_reference,
      Layer:layer,
      Is_Active:is_active,
    }

  await useRequest
    .post(`Category/GetCategoryList`, Payload)
    .then((res) => {
      result.category_list = res.data.data.category_list
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function PostCategoryListAsync(name, layer, is_active, uid_reference) {
  const Payload = JSON.stringify({
    Name: name,
    Layer: layer,
    Is_Active: is_active,
    Uid_Reference: uid_reference,
  })

  const result = {
    is_success: true,
    error: null
  }
  await useRequest.post('Category/AddNewCategory', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}

export async function PatchCategoryListAsync(
  uid,
  name,
  is_active
) {
  const Payload = JSON.stringify({
    Uid: uid,
    Name: name, 
    is_active: is_active,
  })

  const result = {
    is_success: true,
    error: null
  }

  await useRequest.patch('Category/UpdateCategoryList', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}

export async function GetTopIssueSelectedAsync(is_active) {
  const result = {
    is_success: true,
    top_issue_selected: null,
    error: null
  }

    const Payload = {
      Is_Active:is_active,
    }

  await useRequest
    .post(`Category/GetCategoryTopIssueSelected`, Payload)
    .then((res) => {
      result.top_issue_selected = res.data.data.category_top_issue_selected

    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

export async function GetTopIssueAvailableAsync(is_active) {
  const result = {
    is_success: true,
    top_issue_available: null,
    error: null
  }

    const Payload = {
      Is_Active:is_active,
    }

  await useRequest
    .post(`Category/GetCategoryTopIssueAvailable`, Payload)
    .then((res) => {
      result.top_issue_available = res.data.data.category_top_issue_available
      
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}


export async function PostSelectedTopIssueAsync(uid_bot_category) {
  const Payload = JSON.stringify({
    Uid_Bot_Category: uid_bot_category,
  })

  const result = {
    is_success: true,
    error: null
  }
  await useRequest.post('Category/AddSelectedTopIssue', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}

export async function PostAllSelectedTopIssueAsync() {
  const result = {
    is_success: true,
    error: null
  }
  await useRequest.post('Category/AddAllSelectedTopIssue').catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })
  return result
}


export async function DeleteCategoryTopIssueSelectedAsync(
  sequence
) {
  const Payload = {
    Sequence: sequence
  }

  const result = {
    is_success: true,
    error: null
  }

  await useRequest.patch('Category/RemoveSelectedTopIssue', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}

export async function DeleteCategoryTopIssueAllSelectedAsync(
) {
  const result = {
    is_success: true,
    error: null
  }

  await useRequest.patch('Category/RemoveAllSelectedTopIssue').catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}

export async function PatchSequenceTopIssueSelectedAsync(
  current_sequence,
  new_sequence
) {
  const Payload = JSON.stringify({
    Current_Sequence: current_sequence,
    New_Sequence: new_sequence,
  })

  const result = {
    is_success: true,
    error: null
  }

  await useRequest.patch('Category/PatchSequenceSelectedTopIssue', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}