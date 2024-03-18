import { ref } from 'vue'
import { GetDetailContentAsync, PatchContentAsync } from '@/api/reqContent.js'
import { GetCategoryReferenceAsync } from '@/api/reqCategory.js'

const categoryRef = ref([])
const editArticle = ref({
  id: '',
  title: '',
  category_id: '',
  newDescriptionHtml: '',
  newDescription: '',
  newArticle: '',
  additional_link: ''
})

const errorEdit = ref({
  title: { isError: false, message: '' },
  category_id: { isError: false, message: '' },
  newDescription: { isError: false, message: '' },
  newArticle: { isError: false, message: '' }
})

async function GetArticleById(articleId) {
  const result = await GetDetailContentAsync(articleId)
  if (result.is_success) {
    for (const key in result.content) {
      if (key === 'article' || key === 'description') {
        editArticle.value[key] = JSON.parse(result.content[key])
        continue
      }
      editArticle.value[key] = result.content[key]
    }
    return true
  }
  return false
}

async function GetCategoryReference() {
  const result = await GetCategoryReferenceAsync()
  if (result.is_success) {
    categoryRef.value = result.reference.map((item) => {
      return {
        id: item.category_id,
        name: item.category_name
      }
    })
  }
}

async function HandleRePublish() {
  const isValid = ValidateEmptyInput()
  if (!isValid) return false

  const patchArticle = await PatchContentAsync(
    editArticle.value.id,
    editArticle.value.title,
    editArticle.value.category_id,
    editArticle.value.newDescriptionHtml,
    JSON.stringify(editArticle.value.newDescription),
    JSON.stringify(editArticle.value.newArticle),
    editArticle.value.additional_link
  )

  if (!patchArticle.is_success) {
    if (patchArticle.error.statuscode === 400) {
      errorEdit.value.title.isError = true
      errorEdit.value.title.message = patchArticle.error.message
    }
    return patchArticle.error.statuscode
  }

  ResetInput()
  return true
}

function ValidateEmptyInput() {
  let valid = true

  for (const key in errorEdit.value) {
    const inputArticle = editArticle.value[key]
    if (inputArticle === '') {
      errorEdit.value[key].isError = true
      errorEdit.value[key].message = 'Required'
      valid = false
    } else {
      errorEdit.value[key].isError = false
      errorEdit.value[key].message = ''
    }
  }

  return valid
}

function ResetInput() {
  for (const key in editArticle.value) {
    editArticle.value[key] = ''
  }

  for (const key in errorEdit.value) {
    errorEdit.value[key].isError = false
    errorEdit.value[key].message = ''
  }
}

export {
  editArticle,
  categoryRef,
  errorEdit,
  GetArticleById,
  GetCategoryReference,
  HandleRePublish,
  ResetInput
}